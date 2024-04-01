import { Component } from '@angular/core';
import { RegisterRequest } from '../models/register-request.model';
import { Subscription } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  model: RegisterRequest;

  registerSubscription?: Subscription;

  constructor(private authService: AuthService,
    private cookieService: CookieService,
    private router: Router) {
    this.model = {
      email: "",
      password: ""
    };
  }

  onFormSubmit(): void {
    this.registerSubscription = this.authService.register(this.model)
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
      }
    })
  }
}
