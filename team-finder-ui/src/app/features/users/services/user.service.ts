import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';
import { environment } from 'src/environments/environment';
import { UserProfile } from '../models/user-profile.model';
import { UserEditRequest } from '../models/user-edit-request.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  indexUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${environment.apiBaseUrl}/auth/users`);
  }

  deleteUser(id: string) {
    return this.http.delete(`${environment.apiBaseUrl}/auth/users/${id}`);
  }

  addRole(email: string) {
    return this.http.post(`${environment.apiBaseUrl}/auth/addrole`, {
      email: email,
      roleToAdd: "Organizer"
    });
  }

  getUser(id: string): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${environment.apiBaseUrl}/auth/users/${id}`);
  }

  updateUser(id: string, editUserRequest: UserEditRequest): Observable<void> {
    return this.http.put<void>(`${environment.apiBaseUrl}/auth/users/edit/${id}?addAuth=true`, editUserRequest);
  }
}
