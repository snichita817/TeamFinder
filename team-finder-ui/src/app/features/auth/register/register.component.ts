import { Component, NgModule } from '@angular/core';
import { RegisterRequest } from '../models/register-request.model';
import { Subscription } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidationMessagesComponent } from 'src/app/shared/components/errors/validation-messages/validation-messages.component';
export {ValidationMessagesComponent}

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})

export class RegisterComponent {
  model: RegisterRequest;
  registerForm: FormGroup = new FormGroup({});
  submitted = false;
  errorMessages: string[] = [];

  registerSubscription?: Subscription;

  constructor(private authService: AuthService,
    private cookieService: CookieService,
    private router: Router,
    private formBuilder: FormBuilder) {
    this.model = {
      email: "",
      password: ""
    };
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.pattern('^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$')]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]],
    })
  }
  
  onFormSubmit(): void {
    this.submitted = true;
    this.errorMessages = [];

   if(this.registerForm.valid)
   {
      this.registerSubscription = this.authService.register(this.registerForm.value)
      .subscribe({
        next: (response) => {
          this.cookieService.set('Authorization', `Bearer ${response.token}`,
          undefined, '/', undefined, true, 'Strict');

          this.authService.setUser({
            id: response.id,
            email: response.email,
            roles: response.roles
          });

          this.router.navigateByUrl('/');
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

  ngOnDestroy() {
    this.registerSubscription?.unsubscribe();
  }
}
