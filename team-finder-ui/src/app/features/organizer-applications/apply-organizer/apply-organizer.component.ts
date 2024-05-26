import { Component, OnInit } from '@angular/core';
import { OrganizerApplicationService } from '../services/organizer-application.service';
import { Router } from '@angular/router';
import { OrganizerApplicationAdd } from '../models/organizer-application-add-req.model';
import { AuthService } from '../../auth/services/auth.service';
import { SharedService } from 'src/app/shared/shared.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { ChangeEvent } from '@ckeditor/ckeditor5-angular';

@Component({
  selector: 'app-apply-organizer',
  templateUrl: './apply-organizer.component.html',
  styleUrls: ['./apply-organizer.component.css']
})
export class ApplyOrganizerComponent {
  editor = ClassicEditor;

  application: OrganizerApplicationAdd;
  userId?: string;
  applicationForm: FormGroup = new FormGroup({});

  constructor(
    private formBuilder: FormBuilder,
    private organizerApplicationService: OrganizerApplicationService, 
    private router: Router,
    private authService : AuthService,
    private sharedService: SharedService
  ) {
    this.application = {
      userId: '',
      reason: ''
    }
    this.userId = this.authService.getUser()?.id;
    if(this.userId) {
      this.application.userId = this.userId;
    }
    if (!this.userId) {
      this.sharedService.showNotification(false, "Something went wrong!", "Please log in before applying for organizer role!");
      this.router.navigateByUrl('/register');
    }
  }
  public onReady(editor: any) {
    console.log("CKEditor5 Angular Component is ready to use!", editor);
  }
  public onChange({ editor }: ChangeEvent) {
    const data = editor.getData();
  }

  ngOnInit(): void {
    this.applicationForm = this.formBuilder.group({
      reason: ['', [Validators.required, Validators.minLength(10)]]
    });
  }

  submitApplication() {
    if (this.applicationForm.invalid) {
      return;
    }

    this.application.reason = this.applicationForm.get('reason')?.value
    console.log(this.application)
    this.organizerApplicationService.applyForOrganizer(this.application).subscribe(() => {
      this.sharedService.showNotification(true, "Success!", "Application submitted successfully.")
      this.router.navigate(['/']);
    }, error => {
      alert('An error occurred while submitting the application.');
    });
  }

  get reason() {
    return this.applicationForm.get('reason');
  }
}

