import { Component } from '@angular/core';
import { CategoryAddRequest } from '../models/category-add-request.model';
import { CategoryService } from '../services/category.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-category-add',
  templateUrl: './category-add.component.html',
  styleUrls: ['./category-add.component.css']
})
export class CategoryAddComponent {
  model: CategoryAddRequest;
  categoryId: string | null = null;

  private routeSubscription?: Subscription;
  private addCategorySubscription?: Subscription;

  constructor(private categoryService: CategoryService,
    private router: Router) {
      this.model = {
        name: ''
      }
  }

  onFormSubmit() {
    this.addCategorySubscription = this.categoryService.addCategory(this.model)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('admin/activities');
        }
      })
  }

  ngOnDestroy() {
    this.addCategorySubscription?.unsubscribe();
  }
}
