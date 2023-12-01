import { Component, OnDestroy } from '@angular/core';
import { ActivityAddRequest } from '../models/activity-add-request.model';
import { ActivityService } from '../services/activity.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
@Component({
  selector: 'app-activity-add',
  templateUrl: './activity-add.component.html',
  styleUrls: ['./activity-add.component.css']
})
export class ActivityAddComponent implements OnDestroy {
  model: ActivityAddRequest;

  private addActivitySubscription?: Subscription;

  constructor(private activityService: ActivityService,
    private router: Router) {
    this.model = {
      title: '',
      shortDescription: '',
      longDescription: '',
      startDate: new Date(),
      endDate: new Date(),
      openRegistration: true,
      maxParticipants: 0,
      createdBy: ''
    }
  }

  onFormSubmit(){
    this.addActivitySubscription = this.activityService.addActivity(this.model)
    .subscribe({
      next: (response) => {
        this.router.navigateByUrl('');
      }
    })
  }

  ngOnDestroy(): void {
    this.addActivitySubscription?.unsubscribe();
  }
}
