import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddUpdateRequest } from '../models/update-add-request.model';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UpdateService {

  constructor(private http: HttpClient) {
  }

  addUpdate(model: AddUpdateRequest): Observable<void> {
    return this.http.post<void>(`${environment.apiBaseUrl}/updates`, model);
  }
}
