import { Component } from '@angular/core';
import { Team } from '../../teams/models/team.model';
import { ActivityService } from '../services/activity.service';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { TeamService } from '../../teams/services/team.service';
import { SharedService } from 'src/app/shared/shared.service';

@Component({
  selector: 'app-activity-team-review',
  templateUrl: './activity-team-review.component.html',
  styleUrl: './activity-team-review.component.css'
})
export class ActivityTeamReviewComponent {
  activityId: string | null = '';

  teams?: Team[];

  activityServiceSubscription?: Subscription;
  teamServiceSubscription?: Subscription;
  routeSubscription?: Subscription;

  constructor(private activityService: ActivityService,
    private teamService: TeamService,
    private activatedRoute: ActivatedRoute,
    private sharedService: SharedService,
    private router: Router
  ) {}

  ngOnInit() {
    this.routeSubscription = this.activatedRoute.paramMap.subscribe({
      next: (route) => {
        this.activityId = route.get('id')
        if(this.activityId) {
          this.activityServiceSubscription = this.activityService.getTeamsForReview(this.activityId).subscribe({
            next: (response) => {
              this.teams = response;
              console.log(this.teams[0].activityRegistered)
            }
          })
        }
      }
    })
  }

  onAccept(teamId: string, event?: MouseEvent) {
    if(event) {
      event.stopPropagation();
    }

    this.teamServiceSubscription = this.teamService.acceptTeam(teamId).subscribe({
      next: (response) => {
        this.sharedService.showNotification(true, "Success!", `${response.name} accepted to the activity!`)
        this.ngOnInit()
      },
      error: (error) => {
        if(error.error) {
          this.sharedService.showNotification(false, 'Error!', error.error);
        }
      }
    })
  }

  onReject(teamId: string, event?: MouseEvent) {
    if(event) {
      event.stopPropagation();
    }

    this.teamServiceSubscription = this.teamService.rejectTeam(teamId).subscribe({
      next: (response) => {
        this.sharedService.showNotification(true, "Success!", `${response.name} rejected from the activity!`)
        this.ngOnInit()
      },
      error: (error) => {
        if(error.error) {
          this.sharedService.showNotification(false, 'Error!', error.error);
        }
      }
    })
  }

  ngOnDestroy() {
    this.activityServiceSubscription?.unsubscribe();
    this.teamServiceSubscription?.unsubscribe();
    this.routeSubscription?.unsubscribe();
  }
}
