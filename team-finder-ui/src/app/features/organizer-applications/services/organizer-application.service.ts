import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrganizerApplicationAdd } from '../models/organizer-application-add-req.model';
import { OrganizerApplication } from '../models/organizer-application-req.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrganizerApplicationService {
  private apiUrl = `${environment.apiBaseUrl}/OrganizerApplications`;

  constructor(private http: HttpClient) { }

  applyForOrganizer(application: OrganizerApplicationAdd): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}?addAuth=true`, application);
  }

  getApplications(): Observable<OrganizerApplication[]> {
    return this.http.get<OrganizerApplication[]>(`${this.apiUrl}?addAuth=true`);
  }

  getApplication(id: string): Observable<OrganizerApplication> {
    return this.http.get<OrganizerApplication>(`${this.apiUrl}/${id}?addAuth=true`);
  }

  approveApplication(id: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/approve/${id}?addAuth=true`, {});
  }

  rejectApplication(id: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/reject/${id}?addAuth=true`, {});
  }
}
