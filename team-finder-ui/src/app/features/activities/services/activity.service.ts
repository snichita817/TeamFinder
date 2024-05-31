import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivityAddRequest } from '../models/activity-add-request.model';
import { Activity } from '../models/activity.model';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ActivityEditRequest } from '../models/activity-edit-request.model';
import { Team } from '../../teams/models/team.model';
import { PickWinnerAdd } from '../models/pick-winner-add.model';
@Injectable({
  providedIn: 'root'
})
export class ActivityService {

  constructor(private http: HttpClient) { }

  addActivity(model: ActivityAddRequest): Observable<void> {
    return this.http.post<void>(`${environment.apiBaseUrl}/activities?addAuth=true`, model);
  }

  getActivity(id: string): Observable<Activity> {
    return this.http.get<Activity>(`${environment.apiBaseUrl}/activities/${id}`);
  }

  getTeamsForReview(id: string, queryText?: string): Observable<Team[]>{
    let params = new HttpParams();
    if(queryText) {
      params = params.set('query', queryText);
    }

    return this.http.get<Team[]>(`${environment.apiBaseUrl}/teams/review/activity/${id}`, {
      params: params
    });
  }

  indexActivities(queryText?: string, filterText?: string, organizerId?: string): Observable<Activity[]> {
    let params = new HttpParams();
    if(queryText) {
      params = params.set('query', queryText);
    }
    if(filterText) {
      params = params.set('filter', filterText)
    }
    if(organizerId) {
      params = params.set('organizerId', organizerId)
    }
    return this.http.get<Activity[]>(`${environment.apiBaseUrl}/activities`, {
      params: params
    });
    
  }

  updateActivity(id: string, model:ActivityEditRequest): Observable<void> {
    return this.http.put<void>(`${environment.apiBaseUrl}/activities/${id}?addAuth=true`, model);
  }

  deleteActivity(id: string): Observable<Activity> {
    return this.http.delete<Activity>(`${environment.apiBaseUrl}/activities/${id}?addAuth=true`);
  }

  createWinnerResult(winnerResultDto: PickWinnerAdd): Observable<any> {
    return this.http.post(`${environment.apiBaseUrl}/WinnerResults?addAuth=true`, winnerResultDto);
  }
}