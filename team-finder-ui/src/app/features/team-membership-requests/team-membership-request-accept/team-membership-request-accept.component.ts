import { Component } from '@angular/core';
import { TeamMembershipRequestsService } from '../services/team-membership-requests.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { SharedService } from 'src/app/shared/shared.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-team-membership-request-accept',
  templateUrl: './team-membership-request-accept.component.html',
  styleUrl: './team-membership-request-accept.component.css'
})
export class TeamMembershipRequestAcceptComponent {
  response: string | null = '';
  requestId: string | null = '';
  
  activatedRouteSubscription?: Subscription;
  teamMembershipRequestServiceSubscription?: Subscription;

  constructor(private teamMembershipRequestService: TeamMembershipRequestsService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private sharedService: SharedService,
    private location: Location
  ) {}

  ngOnInit() {
    this.activatedRouteSubscription = this.activatedRoute.paramMap.subscribe({
      next: (route) => {
        this.requestId = route.get('memId');
        this.response = route.get('res');

        if(this.requestId && this.response) {
          if(this.response.toLowerCase() == 'accept') {
            this.teamMembershipRequestServiceSubscription = this.teamMembershipRequestService.acceptTeamMembershipReuqest(this.requestId).subscribe({
              next: (response) => {
                this.location.back();
                this.sharedService.showNotification(true, 'Success!', 'User accepted successfully!');
              }
            })
          }
        }
      }
    })
  }

  ngOnDestroy(){
    this.teamMembershipRequestServiceSubscription?.unsubscribe();
    this.activatedRouteSubscription?.unsubscribe();
  } 
}
