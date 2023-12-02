import { Component } from '@angular/core';
import { ActivityService } from '../services/activity.service';
import { Activity } from '../models/activity.model';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-activity-edit',
  templateUrl: './activity-edit.component.html',
  styleUrls: ['./activity-edit.component.css']
})
export class ActivityEditComponent {
  id: string | null = null;
  model?: Activity;

  routeSubscription?: Subscription;
  getActivitySubscription?: Subscription;

  constructor(private route: ActivatedRoute,
    private ActivityService: ActivityService) { 
  }

  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if(this.id){
          this.getActivitySubscription = this.ActivityService.getActivity(this.id).subscribe({
            next: (result) => {
              this.model = result;
            }
          });
        }
      }
    })
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.getActivitySubscription?.unsubscribe();
  }

  onFormSubmit(): void {
  }
}
