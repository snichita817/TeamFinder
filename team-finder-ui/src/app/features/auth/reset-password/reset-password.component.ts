import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { SharedService } from 'src/app/shared/shared.service';
import { AuthService } from '../services/auth.service';
import { Subscription } from 'rxjs';
import { ResetPassword } from '../models/reset-password.model';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent {
  resetPasswordForm: FormGroup = new FormGroup([]);
  token: string | undefined;
  email: string | undefined;
  submitted = false;
  errorMessages: string[] = []

  activatedRouteSubscription?: Subscription;
  authServiceSubscription?: Subscription;

  constructor(private authService: AuthService,
    private sharedService: SharedService,
    private formBuilder: FormBuilder,
    private router: Router,
    private activatedRoute: ActivatedRoute) {}
  
  ngOnInit(): void {
    if(this.authService.getUser() != undefined) {
      this.router.navigateByUrl('/');
    } else {
      this.activatedRouteSubscription = this.activatedRoute.queryParamMap.subscribe({
        next: (params: any) => {
          this.token = params.get('token');
          this.email = params.get('email');

          if(this.token && this.email) {
            this.initializeForm(this.email);
          } else {
            this.router.navigateByUrl('/login');
          }
        }
      })
    }
  }
  
  initializeForm(email: string) {
    this.resetPasswordForm = this.formBuilder.group({
      email: [{value: email, disabled: true}],
      newPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]]
    })
  }

  resetPassword() {
    this.submitted = true;
    this.errorMessages = [];

    if(this.resetPasswordForm.valid && this.email && this.token) {
      const model: ResetPassword = {
        token: this.token,
        email: this.email,
        newPassword: this.resetPasswordForm.get('newPassword')?.value
      };

      this.authServiceSubscription = this.authService.resetPassword(model).subscribe({
        next: (response: any) => {
          this.sharedService.showNotification(true, response.value.title, response.value.message);
          this.router.navigateByUrl('/login');
        }, error: (error) => {
          if(error.error.errors) {
            this.errorMessages = error.error.errors; // this are the errors coming from Backend
          } else {
            this.errorMessages.push(error.error);
          }
        }
      })
    }
  }

  cancel() {
    this.router.navigateByUrl('/login')
  }

  ngOnDestroy() {
    this.activatedRouteSubscription?.unsubscribe();
    this.authServiceSubscription?.unsubscribe();
  }
}
