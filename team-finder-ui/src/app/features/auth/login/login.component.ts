import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Subscription } from 'rxjs';
import { LoginRequest } from '../models/login-request.model';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SharedService } from 'src/app/shared/shared.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  model: LoginRequest;
  loginForm: FormGroup = new FormGroup({});
  submitted = false;
  errorMessages: string[] = [];

  loginSubscription?: Subscription;

  constructor(private authService: AuthService,
    private cookieService: CookieService,
    private sharedService: SharedService,
    private router: Router,
    private formBuilder: FormBuilder) {
    this.model = {
      email: '',
      password: ''
    };
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required]],
      password: ['', [Validators.required]],
    })
  }

  onFormSubmit(): void {
    this.submitted = true;
    this.errorMessages = [];
    
    if(this.loginForm.valid)
    {
      this.loginSubscription = this.authService.login(this.loginForm.value)
      .subscribe({
        next: (response) => {
          // Set Auth cookie
          this.cookieService.set('Authorization', `Bearer ${response.token}`,
          undefined, '/', undefined, true, 'Strict');
          // Set user
          this.authService.setUser({
            id: response.id,
            email: response.email,
            roles: response.roles
          });

          this.router.navigateByUrl('/');
          this.sharedService.showNotification(true, "Welcome Back!", "We glad to have you back with us!");
        },
        error: error => {
          if(error.error.errors) {
            this.errorMessages = error.error.errors; // this are the errors coming from Backend
          } else {
            this.errorMessages.push(error.error);
          }
        }
      })
    }
  }

  ngOnDestroy(): void {
    this.loginSubscription?.unsubscribe();
  }
}