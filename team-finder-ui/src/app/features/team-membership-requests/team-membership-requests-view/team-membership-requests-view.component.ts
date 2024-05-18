import { Component } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { TeamMembershipRequestsService } from '../services/team-membership-requests.service';
import { TeamMembershipRequest } from '../models/team-membership-req.model';
import { SharedService } from 'src/app/shared/shared.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-team-membership-requests-view',
  templateUrl: './team-membership-requests-view.component.html',
  styleUrl: './team-membership-requests-view.component.css'
})
export class TeamMembershipRequestsViewComponent {
  teamId: string | null = '';
  teamMembershipRequests?: TeamMembershipRequest[];

  activatedRouteSubscription?: Subscription;
  teamMembershipServiceSubscription?: Subscription;

  constructor(private activatedRoute: ActivatedRoute,
    private teamMembershipService: TeamMembershipRequestsService,
    private sharedService: SharedService,
    private router: Router
  ) {

  }

  ngOnInit() {
    this.activatedRouteSubscription = this.activatedRoute.paramMap.subscribe({
      next: (route) => {
        this.teamId = route.get('teamId');

        if(this.teamId != null) {
          this.teamMembershipServiceSubscription = this.teamMembershipService.getTeamMembershipRequests(this.teamId).subscribe({
            next: (response) => {
              this.teamMembershipRequests = response;
            }
          })
        }
      }
    })
  }

  onAccept(requestId:string, userName: string, event?: MouseEvent) {
    if(event) {
      event.stopPropagation()
    }
    this.teamMembershipServiceSubscription = this.teamMembershipService.acceptTeamMembershipReuqest(requestId).subscribe({
      next: (response) => {
        window.location.reload();
        this.sharedService.showNotification(true, 'Success!', `User ${userName} accepted successfully!`);
      }
    })
  }

  onReject(requestId: string, userName: string, event?: MouseEvent) {
    if(event) {
      event.stopPropagation()
    }
    this.teamMembershipServiceSubscription = this.teamMembershipService.declineTeamMembershipReuqest(requestId).subscribe({
      next: (response) => {
        window.location.reload();
        this.sharedService.showNotification(true, 'Success!', `User ${userName} declined successfully!`);
      }
    })
  }

  ngOnDestroy() {
    this.activatedRouteSubscription?.unsubscribe();
    this.teamMembershipServiceSubscription?.unsubscribe();
  }
}