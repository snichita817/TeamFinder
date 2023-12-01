import { Component, OnInit } from '@angular/core';
import { ActivityService } from '../services/activity.service';
import { Activity } from '../models/activity.model';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-activity-list',
  templateUrl: './activity-list.component.html',
  styleUrls: ['./activity-list.component.css']
})
export class ActivityListComponent implements OnInit {
  activities$?: Observable<Activity[]>;
  
  constructor(private activityService: ActivityService) {
  }

  ngOnInit(): void {
    this.activities$ = this.activityService.indexActivities();
  }

}
