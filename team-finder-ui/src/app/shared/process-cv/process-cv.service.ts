import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CvInfo } from 'src/app/features/users/models/cv-info.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProcessCvService {

  constructor(private http: HttpClient) { }

  uploadCv(formData?: FormData): Observable<CvInfo> {
    return this.http.post<CvInfo>(`${environment.apiBaseUrl}/Cv/upload`, formData);
  }
}
