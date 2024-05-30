import { HttpClient, HttpParams } from '@angular/common/http';
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
    return this.http.get<Team>(`${environment.apiBaseUrl}/teams/${id}?addAuth=true`);
  }

  getAllTeams(queryText?: string): Observable<Team[]> {
    let params = new HttpParams();
    if(queryText) {
      params = params.set('query', queryText);
    }
    return this.http.get<Team[]>(`${environment.apiBaseUrl}/teams`, {
      params: params
    });
  }

  getUserTeams(): Observable<Team[]> {
    return this.http.get<Team[]>(`${environment.apiBaseUrl}/teams/myteams?addAuth=true`)
  }

  editTeam(request: TeamEditRequest, id: string): Observable<void> {
    return this.http.put<void>(`${environment.apiBaseUrl}/teams/${id}?addAuth=true`, request)
  }

  deleteTeam(id: string) {
    return this.http.delete<Team>(`${environment.apiBaseUrl}/teams/${id}?addAuth=true`);
  }

  indexByActivity(activityId: string, queryText?: string): Observable<Team[]> {
    let params = new HttpParams();
    if(queryText) {
      params = params.set('query', queryText);
    }

    return this.http.get<Team[]>(`${environment.apiBaseUrl}/teams/activity/${activityId}`, {
      params: params
    });
  }

  acceptTeam(teamId: string): Observable<Team> {
    return this.http.put<Team>(`${environment.apiBaseUrl}/teams/review/${teamId}/accept?addAuth=true`, {});
  }

  rejectTeam(teamId: string): Observable<Team> {
    return this.http.put<Team>(`${environment.apiBaseUrl}/teams/review/${teamId}/reject?addAuth=true`, {});
  }

  changeSubmissionUrl(teamId: string, submissionUrl: string) {
    return this.http.put(`${environment.apiBaseUrl}/teams/${teamId}/upload/${submissionUrl}?addAuth=true`, {})
  }
}
