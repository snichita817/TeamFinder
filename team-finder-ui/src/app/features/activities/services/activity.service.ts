import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivityAddRequest } from '../models/activity-add-request.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {

  constructor(private http: HttpClient) { }

  addActivity(model: ActivityAddRequest): Observable<void> {
    return this.http.post<void>('https://localhost:7001/api/activities', model);
  }
}
