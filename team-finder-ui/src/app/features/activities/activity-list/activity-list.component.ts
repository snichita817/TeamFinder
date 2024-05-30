import { Component, OnInit } from '@angular/core';
import { ActivityService } from '../services/activity.service';
import { Activity } from '../models/activity.model';
import { Observable } from 'rxjs';
import { AuthService } from '../../auth/services/auth.service';


@Component({
  selector: 'app-activity-list',
  templateUrl: './activity-list.component.html',
  styleUrls: ['./activity-list.component.css']
})
export class ActivityListComponent implements OnInit {
  activities$?: Observable<Activity[]>;
  
  constructor(private activityService: ActivityService,
    private authService: AuthService
  ) {
  }

  ngOnInit(): void {
    this.activities$ = this.activityService.indexActivities();

    this.activities$.subscribe({
      next: (activities) => {
        console.log('Activities:', activities);
      },
      error: (error) => {
        console.error('Error fetching activities:', error);
      }
    });
  }

  verifyOrganizer(): boolean {
    const user = this.authService.getUser();
    if (!user || !user.roles) {
      return false;
    }
    return user.roles.includes("Organizer") || user.roles.includes("Admin");
  }

  onSearch(queryText: string) {
    this.activities$ = this.activityService.indexActivities(queryText);

    this.activities$.subscribe({
      next: (activities) => {
        console.log('Activities:', activities);
      },
      error: (error) => {
        console.error('Error fetching activities:', error);
      }
    });
  }
}
