import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ActivityService } from '../services/activity.service';

@Component({
  selector: 'app-activity-delete',
  templateUrl: './activity-delete.component.html',
  styleUrls: ['./activity-delete.component.css']
})
export class ActivityDeleteComponent {
  id: string | null = null;
  
  routeSubscription?: Subscription;
  deleteActivitySubscription?: Subscription;

  constructor(private route: ActivatedRoute,
    private activityService: ActivityService,
    private router: Router) {
  }

  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if(this.id) {
          this.deleteActivitySubscription = this.activityService.deleteActivity(this.id).subscribe({
            next: (response) => {
              this.router.navigateByUrl('');
            }
          })
        }
      }
    });
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.deleteActivitySubscription?.unsubscribe();
  }
}
