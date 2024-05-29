import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { ActivityService } from '../services/activity.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { Category } from '../../categories/models/category.model';
import { CategoryService } from '../../categories/services/category.service';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { ChangeEvent } from '@ckeditor/ckeditor5-angular';
import { ActivityEditRequest } from '../models/activity-edit-request.model';

@Component({
  selector: 'app-activity-edit',
  templateUrl: './activity-edit.component.html',
  styleUrls: ['./activity-edit.component.css']
})
export class ActivityEditComponent implements OnInit, OnDestroy {
  public Editor = ClassicEditor;
  
  activityForm: FormGroup;
  categories$?: Observable<Category[]>;
  private routeSubscription?: Subscription;
  private getActivitySubscription?: Subscription;
  private editActivitySubscription?: Subscription;
  submitted = false;

  previousValues = {
    minParticipant: undefined as number | undefined,
    maxParticipant: undefined as number | undefined,
    maxTeams: undefined as number | undefined,
  };

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private activityService: ActivityService,
    private categoryService: CategoryService,
    private router: Router
  ) {
    this.activityForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(5)]],
      shortDescription: ['', [Validators.required]],
      longDescription: [''],
      startDate: ['', [Validators.required, this.futureDateValidator]],
      endDate: ['', [Validators.required, this.futureDateValidator]],
      openRegistration: [true],
      minParticipant: ['', [Validators.required, Validators.min(1), this.minimumValueValidator.bind(this)]],
      maxParticipant: ['', [Validators.required, Validators.min(1), this.maximumValueValidator.bind(this)]],
      maxTeams: ['', [Validators.required, Validators.min(1), this.maximumTeamsValidator.bind(this)]],
      categories: [[]]
    }, { validators: [this.dateRangeValidator] });
  }

  ngOnInit(): void {
    this.categories$ = this.categoryService.indexCategories();
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');
        if (id) {
          this.getActivitySubscription = this.activityService.getActivity(id).subscribe({
            next: (result) => {
              this.activityForm.patchValue({
                title: result.title,
                shortDescription: result.shortDescription,
                longDescription: result.longDescription,
                startDate: result.startDate,
                endDate: result.endDate,
                openRegistration: result.openRegistration,
                minParticipant: result.minParticipant,
                maxParticipant: result.maxParticipant,
                maxTeams: result.maxTeams,
                categories: result.categories.map(category => category.id)
              });

              this.previousValues.minParticipant = result.minParticipant;
              this.previousValues.maxParticipant = result.maxParticipant;
              this.previousValues.maxTeams = result.maxTeams;
            }
          });
        }
      }
    });
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.getActivitySubscription?.unsubscribe();
    this.editActivitySubscription?.unsubscribe();
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

  minimumValueValidator(control: AbstractControl): ValidationErrors | null {
    const minValue = control.value;
    if (this.previousValues.minParticipant === undefined || minValue <= this.previousValues.minParticipant) {
      return null;
    }
    return { minValue: true };
  }

  maximumValueValidator(control: AbstractControl): ValidationErrors | null {
    const maxValue = control.value;
    if (this.previousValues.maxParticipant === undefined || maxValue >= this.previousValues.maxParticipant) {
      return null;
    }
    return { maxValue: true };
  }

  maximumTeamsValidator(control: AbstractControl): ValidationErrors | null {
    const maxValue = control.value;
    if (this.previousValues.maxTeams === undefined || maxValue >= this.previousValues.maxTeams) {
      return null;
    }
    return { maxTeamsValue: true };
  }

  public onReady(editor: any): void {
    console.log("CKEditor5 Angular Component is ready to use!", editor);
  }

  public onChange({ editor }: ChangeEvent): void {
    const data = editor.getData();
    this.activityForm.get('longDescription')?.setValue(data);
  }

  onFormSubmit(): void {
    this.submitted = true;
    if (this.activityForm.invalid) {
      return;
    }

    const formValue = this.activityForm.value;
    const editActivityRequest: ActivityEditRequest = {
      ...formValue,
      createdBy: localStorage.getItem('user-id') || ''
    };

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.editActivitySubscription = this.activityService.updateActivity(id, editActivityRequest).subscribe({
        next: (response) => {
          this.router.navigateByUrl(`/activities/get/${id}`);
        }
      });
    }
  }
}
