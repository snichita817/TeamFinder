import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StorageService {

  constructor(private httpClient: HttpClient) { }

  addFile(formData: FormData): Observable<string> {
    return this.httpClient.post(`${environment.apiBaseUrl}/storage/upload`, formData, { responseType: 'text' });
  }
}
