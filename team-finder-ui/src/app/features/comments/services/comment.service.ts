import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AddCommentDto } from '../models/add-comment.model';
import { Comment } from '../models/comment.model';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(private http: HttpClient) { }

  getComments(updateId: string): Observable<Comment[]> {
    return this.http.get<Comment[]>(`${environment.apiBaseUrl}/comments/${updateId}`);
  }

  postComment(comment: AddCommentDto): Observable<Comment> {
    return this.http.post<Comment>(`${environment.apiBaseUrl}/comments`, comment);
  }

  deleteComment(commentId: string): Observable<void> {
    return this.http.delete<void>(`${environment.apiBaseUrl}/comments/${commentId}`);
  }
}
