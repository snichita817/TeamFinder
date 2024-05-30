import { Component } from '@angular/core';
import { AuthService } from '../../auth/services/auth.service';
import { TeamMembershipRequest } from '../models/team-membership-req.model';
import { TeamMembershipRequestsService } from '../services/team-membership-requests.service';
import { SharedService } from 'src/app/shared/shared.service';
import { TeamMembershipRequestsViewComponent } from '../team-membership-requests-view/team-membership-requests-view.component';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-get-user-requests',
  templateUrl: './get-user-requests.component.html',
  styleUrl: './get-user-requests.component.css'
})
export class GetUserRequestsComponent {
  membershipRequests: TeamMembershipRequest[] = [];

  membershipRequestServiceSubscription?: Subscription;

  constructor(private authService: AuthService,
    private membershipService: TeamMembershipRequestsService,
    private sharedService: SharedService,
    private router: Router
  ) {}

  ngOnInit() {
    if(this.authService.getUser() == undefined) {
      this.sharedService.showNotification(false, "Error!", "You should log in before accessing this page");
      this.router.navigateByUrl('login')
    }
    this.membershipRequestServiceSubscription = this.membershipService.getUserTeamMembershipRequest().subscribe({
      next: (response) => {
        this.membershipRequests = response;
        console.log(this.membershipRequests)
      }
    })

  }

  ngOnDestroy() {
    this.membershipRequestServiceSubscription?.unsubscribe();
  }
}
