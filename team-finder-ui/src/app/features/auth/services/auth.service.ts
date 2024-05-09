import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { LoginRequest } from '../models/login-request.model';
import { LoginResponse } from '../models/login-response.model';
import { User } from '../models/user.model';
import { CookieService } from 'ngx-cookie-service';
import { RegisterRequest } from '../models/register-request.model';
import { RegisterResponse } from '../models/register-response';
import { ConfirmEmail } from '../models/confirm-email.model';
import { ResetPassword } from '../models/reset-password.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  $user = new BehaviorSubject<User | undefined>(undefined);

  constructor(private http: HttpClient,
    private cookeService: CookieService) { }

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiBaseUrl}/auth/login`, {
      email: request.email,
      password: request.password
    });
  }

  register(request: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${environment.apiBaseUrl}/auth/register`, {
      email: request.email,
      password: request.password
    });
  }

  confirmEmail(request: ConfirmEmail): Observable<LoginResponse> {
    return this.http.put<LoginResponse>(`${environment.apiBaseUrl}/auth/confirm-email`, request);
  }

  resendEmailConfirmationLink(email: string) {
    return this.http.post(`${environment.apiBaseUrl}/auth/resend-email-confirmation/${email}`, {});
  }

  forgotPassword(email: string) {
    return this.http.post(`${environment.apiBaseUrl}/auth/forgot-password/${email}`, {});
  }

  resetPassword(model: ResetPassword) {
    return this.http.put(`${environment.apiBaseUrl}/auth/reset-password`, model);
  }

  setUser(user: User): void {
    this.$user.next(user);
    localStorage.setItem('user-id', user.id);
    localStorage.setItem('user-email', user.email);
    localStorage.setItem('user-roles', user.roles.join(','));
  }

  getUser(): User | undefined {
    const email = localStorage.getItem('user-email');
    const roles = localStorage.getItem('user-roles');
    const id = localStorage.getItem('user-id');

    if(id && email && roles) {
      const user: User = {
        id: id,
        email: email,
        roles: roles.split(',')
      }
      return user;
    }

    return undefined;
  }

  user(): Observable<User | undefined> {
    return this.$user.asObservable();
  }

  logout(): void {
    localStorage.clear();
    this.cookeService.delete('Authorization', '/');
    this.$user.next(undefined);
  }
}
