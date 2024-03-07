import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from '../models/category.model';
import { CategoryService } from '../services/category.service';

@Component({
  selector: 'app-categories-list-private',
  templateUrl: './categories-list-private.component.html',
  styleUrls: ['./categories-list-private.component.css']
})
export class CategoriesListPrivateComponent {
  categories$?: Observable<Category[]>;

  constructor(private categoryService: CategoryService) {}

  ngOnInit(): void {
    this.categories$ = this.categoryService.indexCategories();
  }
}
