import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { ActivityAddRequest } from '../models/activity-add-request.model';
import { ActivityService } from '../services/activity.service';
import { Observable, Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { Category } from '../../categories/models/category.model';
import { CategoryService } from '../../categories/services/category.service';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { ChangeEvent } from '@ckeditor/ckeditor5-angular';

@Component({
  selector: 'app-activity-add',
  templateUrl: './activity-add.component.html',
  styleUrls: ['./activity-add.component.css']
})
export class ActivityAddComponent implements OnInit, OnDestroy {
  public Editor = ClassicEditor;

  activityForm: FormGroup;
  categories$?: Observable<Category[]>;
  private addActivitySubscription?: Subscription;
  submitted = false;
  errorMessages: string[] = [];

  constructor(
    private fb: FormBuilder,
    private activityService: ActivityService,
    private categoryService: CategoryService,
    private router: Router
  ) {
    this.activityForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(5)]],
      shortDescription: ['', [Validators.required, Validators.maxLength(255)]],
      longDescription: [''],
      startDate: ['', [Validators.required, this.futureDateValidator]],
      endDate: ['', [Validators.required, this.futureDateValidator]],
      openRegistration: [true],
      minParticipant: ['', [Validators.required, Validators.min(1)]],
      maxParticipant: ['', [Validators.required, Validators.min(1)]],
      maxTeams: ['', [Validators.required, Validators.min(1)]],
      categories: [[]]
    }, { validators: this.dateRangeValidator });
  }

  ngOnInit(): void {
    this.categories$ = this.categoryService.indexCategories();
  }

  onFormSubmit(): void {
    this.submitted = true;
    if (this.activityForm.invalid) {
      return;
    }

    const formValue = this.activityForm.value;
    const activity: ActivityAddRequest = {
      ...formValue,
      createdBy: localStorage.getItem('user-id') || ''
    };

    this.addActivitySubscription = this.activityService.addActivity(activity)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('/activities');
        },
        error: (error) => {
          this.errorMessages = [error.message];
        }
      });
  }

  futureDateValidator(control: AbstractControl): ValidationErrors | null {
    const today = new Date().setHours(0, 0, 0, 0);
    const controlDate = new Date(control.value).setHours(0, 0, 0, 0);
    return controlDate >= today ? null : { pastDate: true };
  }

  dateRangeValidator(group: AbstractControl): ValidationErrors | null {
    const startDate = group.get('startDate')?.value;
    const endDate = group.get('endDate')?.value;
    if (startDate && endDate) {
      return new Date(startDate) < new Date(endDate) ? null : { dateRange: true };
    }
    return null;
  }
  
  public onReady(editor: any) {
    console.log("CKEditor5 Angular Component is ready to use!", editor);
  }

  public onChange({ editor }: ChangeEvent) {
    const data = editor.getData();
  }

  ngOnDestroy(): void {
    this.addActivitySubscription?.unsubscribe();
  }
}
