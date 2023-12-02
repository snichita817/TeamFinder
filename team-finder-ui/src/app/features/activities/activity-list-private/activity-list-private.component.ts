import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { Activity } from '../models/activity.model';
import { ActivityService } from '../services/activity.service';

@Component({
  selector: 'app-activity-list-private',
  templateUrl: './activity-list-private.component.html',
  styleUrls: ['./activity-list-private.component.css']
})
export class ActivityListPrivateComponent {
  activities$?: Observable<Activity[]>;

  constructor(private activityService: ActivityService) {
  }

  ngOnInit(): void {
    this.activities$ = this.activityService.indexActivities();
  }
}
