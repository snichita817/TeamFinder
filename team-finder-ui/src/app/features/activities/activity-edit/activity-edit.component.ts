import { Component } from '@angular/core';
import { ActivityService } from '../services/activity.service';
import { Activity } from '../models/activity.model';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ActivityEditRequest } from '../models/activity-edit-request.model';

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
  editActivitySubscription?: Subscription;

  constructor(private route: ActivatedRoute,
    private activityService: ActivityService,
    private router: Router) { 
  }

  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if(this.id){
          this.getActivitySubscription = this.activityService.getActivity(this.id).subscribe({
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
    this.editActivitySubscription?.unsubscribe();
  }

  onFormSubmit(): void {
    const editActivityRequest: ActivityEditRequest = {
      title: this.model?.title ?? '',
      shortDescription: this.model?.shortDescription ?? '',
      longDescription: this.model?.longDescription ?? '',
      startDate: this.model?.startDate ?? new Date(),
      endDate: this.model?.endDate ?? new Date(),
      openRegistration: this.model?.openRegistration ?? true,
      maxParticipant: this.model?.maxParticipant ?? 0,
      createdBy: this.model?.createdBy ?? ''
    }

    if(this.id) {
      this.editActivitySubscription = this.activityService.updateActivity(this.id, editActivityRequest).subscribe({
        next: (response) => {
          this.router.navigateByUrl('');
        },
      });
    }

  }
}
