import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';
import { environment } from 'src/environments/environment';

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
}
