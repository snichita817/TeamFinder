import { Component, OnDestroy } from '@angular/core';
import { ActivityAddRequest } from '../models/activity-add-request.model';
import { ActivityService } from '../services/activity.service';
import { Observable, Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { Category } from '../../categories/models/category.model';
import { CategoryService } from '../../categories/services/category.service';
@Component({
  selector: 'app-activity-add',
  templateUrl: './activity-add.component.html',
  styleUrls: ['./activity-add.component.css']
})
export class ActivityAddComponent implements OnDestroy {
  model: ActivityAddRequest;
  categories$?: Observable<Category[]>; 

  private addActivitySubscription?: Subscription;

  constructor(private activityService: ActivityService,
    private categoryService: CategoryService,
    private router: Router) {
    const userId = localStorage.getItem('user-id');
    this.model = {
      title: '',
      shortDescription: '',
      longDescription: '',
      startDate: new Date(),
      endDate: new Date(),
      openRegistration: true,
      maxParticipant: 0,
      createdBy: userId !== null ? userId : '',
      categories: []
    }
  }

  ngOnInit(): void {
    console.log(this.model);
    this.categories$ = this.categoryService.indexCategories();
  }

  onFormSubmit(){
    this.addActivitySubscription = this.activityService.addActivity(this.model)
    .subscribe({
      next: (response) => {
        this.router.navigateByUrl('/activities');
      }
    })
  }

  ngOnDestroy(): void {
    this.addActivitySubscription?.unsubscribe();
  }
}
