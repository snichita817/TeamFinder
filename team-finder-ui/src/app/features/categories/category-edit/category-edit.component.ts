import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { CategoryService } from '../services/category.service';
import { CategoryEditRequest } from '../models/category-edit-request.model';

@Component({
  selector: 'app-category-edit',
  templateUrl: './category-edit.component.html',
  styleUrls: ['./category-edit.component.css']
})
export class CategoryEditComponent {
  model?: CategoryEditRequest;

  routeSubscription?: Subscription;
  categoryServiceSubscription?: Subscription;
  editCategoryServiceSubscription?: Subscription;

  id: string | null = "";

  constructor(private route: ActivatedRoute,
    private categoryService: CategoryService,
    private router: Router) {}

  ngOnInit() {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if(this.id) {
          this.categoryServiceSubscription = this.categoryService.getCategory(this.id).subscribe({
            next: (response) => {
              this.model = response;
            }
          })
        }
      }
    })
  }

  ngOnDestroy() {
    this.routeSubscription?.unsubscribe();
    this.categoryServiceSubscription?.unsubscribe();
    this.editCategoryServiceSubscription?.unsubscribe();
  }

  onFormSubmit(): void {
    if(this.model && this.id)
    {
      var editedCategory: CategoryEditRequest = {
        name: this.model.name
      }
    }

    this.editCategoryServiceSubscription = this.categoryService.editCategory(this.id, this.model)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('');
        }
      })
  }
}
