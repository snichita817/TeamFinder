import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddUpdateRequest } from '../models/update-add-request.model';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Update } from '../models/update.model';

@Injectable({
  providedIn: 'root'
})
export class UpdateService {

  constructor(private http: HttpClient) {
  }

  addUpdate(model: AddUpdateRequest): Observable<void> {
    return this.http.post<void>(`${environment.apiBaseUrl}/updates`, model);
  }

  indexUpdates(): Observable<Update[]> {
    return this.http.get<Update[]>(`${environment.apiBaseUrl}/updates`);
  }

  deleteUpdate(id: string): Observable<void>{
    return this.http.delete<void>(`${environment.apiBaseUrl}/updates/${id}`);
  }
}
