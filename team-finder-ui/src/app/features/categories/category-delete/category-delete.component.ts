import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { CategoryService } from '../services/category.service';

@Component({
  selector: 'app-category-delete',
  templateUrl: './category-delete.component.html',
  styleUrls: ['./category-delete.component.css']
})
export class CategoryDeleteComponent {
  id: string | null = null;

  routeSubscription?: Subscription;
  deleteCategorySubscription?: Subscription;

  constructor(private route: ActivatedRoute,
    private categoryService: CategoryService,
    private router: Router) {}

  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if(this.id) {
          this.deleteCategorySubscription = this.categoryService.deleteCategory(this.id).subscribe({
            next: (response) => {
              this.router.navigateByUrl('');
            }
          })
        }
      }
    })
  }
  
  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.deleteCategorySubscription?.unsubscribe();
  }
}
