import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Subscription } from 'rxjs';
import { LoginRequest } from '../models/login-request.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  model: LoginRequest;

  loginSubscription?: Subscription;

  constructor(private authService: AuthService) {
    this.model = {
      email: '',
      password: ''
    };
  }

  onFormSubmit(): void {
    this.loginSubscription = this.authService.login(this.model)
    .subscribe({
      next: (response) => {
        console.log(response)
      }
    })
  }

  ngOnDestroy(): void {
    this.loginSubscription?.unsubscribe();
  }
}