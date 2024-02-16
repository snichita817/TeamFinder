import { Component, OnInit } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { Activity } from '../models/activity.model';
import { ActivityService } from '../services/activity.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Update } from '../../updates/models/update.model';

@Component({
  selector: 'app-activity-get',
  templateUrl: './activity-get.component.html',
  styleUrls: ['./activity-get.component.css']
})
export class ActivityGetComponent implements OnInit {
  activityId: string | null = null;
  model?: Activity;

  routeSubscription?: Subscription;
  getActivitySubscription?: Subscription;

  constructor(private activityService: ActivityService,
    private route:ActivatedRoute,
    private router: Router) {
  }

  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.activityId = params.get('id');

        if(this.activityId){
          this.getActivitySubscription = this.activityService.getActivity(this.activityId).subscribe({
            next: (result) => {
              this.model = result;
            }
          });
        }
      }
    });
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.getActivitySubscription?.unsubscribe();
  }

  navigateToUpdate(id: string, event?:MouseEvent) {
    if(event) {
      event.stopPropagation();
    }

    this.router.navigate(['/updates/get', id]);
  }

  navigateToEditUpdate(id: string, event?:MouseEvent) { 
    if(event) {
      event.stopPropagation();
    }
    
    this.router.navigate(['updates/edit', id]);
  }

  navigateToDeleteUpdate(id: string, event?:MouseEvent) { 
    if(event) {
      event.stopPropagation();
    }
    this.router.navigate(['/updates/delete', id]);
  }
}
