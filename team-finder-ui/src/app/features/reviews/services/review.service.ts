// src/app/services/review.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Review } from '../models/review.model';
import { AddReviewDto } from '../models/add-review.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  constructor(private http: HttpClient) { }

  getReviews(organizerId: string): Observable<Review[]> {
    return this.http.get<Review[]>(`${environment.apiBaseUrl}/reviews/${organizerId}`);
  }

  postReview(review: AddReviewDto): Observable<Review> {
    return this.http.post<Review>(`${environment.apiBaseUrl}/reviews`, review);
  }

  deleteReview(id: string): Observable<boolean> {
    return this.http.delete<boolean>(`${environment.apiBaseUrl}/reviews/${id}`);
  }
}
