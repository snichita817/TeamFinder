import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TeamAddRequest } from '../models/team-add-request.model';
import { Observable } from 'rxjs';
import { Team } from '../models/team.model';
import { environment } from 'src/environments/environment';
import { TeamEditRequest } from '../models/team-edit-request.model';

@Injectable({
  providedIn: 'root'
})
export class TeamService {

  constructor(private http: HttpClient) { }

  addTeam(request: TeamAddRequest): Observable<void> {
    return this.http.post<void>(`${environment.apiBaseUrl}/teams/`, request);
  }

  getTeam(id: string): Observable<Team> {
    return this.http.get<Team>(`${environment.apiBaseUrl}/teams/${id}`);
  }

  getAllTeams(): Observable<Team[]> {
    return this.http.get<Team[]>(`${environment.apiBaseUrl}/teams`);
  }

  editTeam(request: TeamEditRequest, id: string): Observable<void> {
    return this.http.put<void>(`${environment.apiBaseUrl}/teams/${id}`, request)
  }

  indexByActivity(activityId: string): Observable<Team[]> {
    return this.http.get<Team[]>(`${environment.apiBaseUrl}/teams/activity/${activityId}`);
  }
}
