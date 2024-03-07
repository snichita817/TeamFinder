import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from '../models/category.model';
import { CategoryEditRequest } from '../models/category-edit-request.model';
import { environment } from 'src/environments/environment';
import { CategoryAddRequest } from '../models/category-add-request.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient) { }

  indexCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${environment.apiBaseUrl}/categories`);
  }

  deleteCategory(id: string): Observable<void> {
    return this.http.delete<void>(`${environment.apiBaseUrl}/categories/${id}`);
  }

  getCategory(id: string): Observable<Category> {
    return this.http.get<Category>(`${environment.apiBaseUrl}/categories/${id}`);
  }

  editCategory(id: string | null, editedCategory: CategoryEditRequest | undefined): Observable<Category> {
    return this.http.put<Category>(`${environment.apiBaseUrl}/categories/${id}`, editedCategory);
  }

  addCategory(model: CategoryAddRequest): Observable<void> {
    return this.http.post<void>(`${environment.apiBaseUrl}/categories`, model);
  }
}
