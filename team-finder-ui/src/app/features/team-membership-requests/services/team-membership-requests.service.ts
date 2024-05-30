import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TeamMembershipRequestsAdd } from '../models/team-membership-add-req.model';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { TeamMembershipRequest } from '../models/team-membership-req.model';

@Injectable({
  providedIn: 'root'
})
export class TeamMembershipRequestsService {

  constructor(private http: HttpClient) { }

  addTeamMembershipRequest(request: TeamMembershipRequestsAdd): Observable<void> {
    return this.http.post<void>(`${environment.apiBaseUrl}/teams/team-membership-requests?addAuth=true`, request);
  }

  getTeamMembershipRequests(teamId: string, queryText?: string): Observable<TeamMembershipRequest[]> {
    let params = new HttpParams();
    if(queryText) {
      params = params.set('query', queryText);
    }
    return this.http.get<TeamMembershipRequest[]>(`${environment.apiBaseUrl}/teams/${teamId}/team-membership-requests?addAuth=true`, {
      params: params
    })
  }

  acceptTeamMembershipReuqest(requestId: string): Observable<void> {
    return this.http.put<void>(`${environment.apiBaseUrl}/teams/team-membership-requests/${requestId}/accept?addAuth=true`, {});
  }

  declineTeamMembershipReuqest(requestId: string): Observable<void> {
    return this.http.put<void>(`${environment.apiBaseUrl}/teams/team-membership-requests/${requestId}/reject?addAuth=true`, {});
  }

  getUserTeamMembershipRequest(): Observable<TeamMembershipRequest[]> {
    return this.http.get<TeamMembershipRequest[]>(`${environment.apiBaseUrl}/teams/my-team-membership-requests?addAuth=true`)
  }
}
