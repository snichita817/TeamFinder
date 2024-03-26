import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Subscription } from 'rxjs';
import { LoginRequest } from '../models/login-request.model';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  model: LoginRequest;

  loginSubscription?: Subscription;

  constructor(private authService: AuthService,
    private cookieService: CookieService,
    private router: Router) {
    this.model = {
      email: '',
      password: ''
    };
  }

  onFormSubmit(): void {
    this.loginSubscription = this.authService.login(this.model)
    .subscribe({
      next: (response) => {
        // Set Auth cookie
        this.cookieService.set('Authorization', `Bearer ${response.token}`,
        undefined, '/', undefined, true, 'Strict');

        // Set user
        this.authService.setUser({
          email: response.email,
          roles: response.roles
        });

        this.router.navigateByUrl('/');
      }
    })
  }

  ngOnDestroy(): void {
    this.loginSubscription?.unsubscribe();
  }
}