import { Injectable } from '@angular/core';
import { LoginRequest } from '../login/models/login-request.model';
import { Observable } from 'rxjs';
import { LoginResponse } from '../login/models/login-response.model';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiBaseUrl}/auth/login`, {
      email: request.email,
      password: request.password
    });
  }
}
