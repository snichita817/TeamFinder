import { Component } from '@angular/core';
import { TeamMembershipRequestsService } from '../services/team-membership-requests.service';
import { TeamMembershipRequestsAdd } from '../models/team-membership-add-req.model';
import { SharedService } from 'src/app/shared/shared.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-team-membership-requests-add',
  templateUrl: './team-membership-requests-add.component.html',
  styleUrl: './team-membership-requests-add.component.css'
})
export class TeamMembershipRequestsAddComponent {
  teamId: string | null = '';

  model: TeamMembershipRequestsAdd;

  activatedRouteSubscription?: Subscription;
  membershipServiceSubscription?: Subscription;

  constructor(private teamMembershipService: TeamMembershipRequestsService,
    private sharedService: SharedService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {
    const userId = localStorage.getItem('user-id');
    this.model = {
      userId: '',
      teamId: ''
    };
    if(userId) {
      this.model.userId = userId;
    }
    else {
      this.sharedService.showNotification(false, "An error occured!", "Please login before trying to apply for a team");
      this.router.navigateByUrl('/login');
    }
  }

  ngOnInit() {
    this.activatedRouteSubscription = this.activatedRoute.paramMap.subscribe({
      next: (route) => {
        this.teamId = route.get('teamId');
        if(this.teamId) {
          this.model.teamId= this.teamId;
          this.membershipServiceSubscription = this.teamMembershipService.addTeamMembershipRequest(this.model).subscribe({
            next: (response: any) => {
              this.sharedService.showNotification(true, 'Success!', 'Join request created successfully!');
              this.router.navigateByUrl(`/team/view/${this.teamId}`);
            },
            error: (error: any) => {
              if(error.error) {
                this.sharedService.showNotification(false, 'Error!', error.error);
                this.router.navigateByUrl('/');
              }
            }
          })
        }
      }
    })
  }

  ngOnDestroy() {
    this.activatedRouteSubscription?.unsubscribe();
    this.membershipServiceSubscription?.unsubscribe();
  }
}
