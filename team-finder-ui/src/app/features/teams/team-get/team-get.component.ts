import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TeamService } from '../services/team.service';
import { Subscription } from 'rxjs';
import { Team } from '../models/team.model';
import { SharedService } from 'src/app/shared/shared.service';

@Component({
  selector: 'app-team-get',
  templateUrl: './team-get.component.html',
  styleUrl: './team-get.component.css'
})

export class TeamGetComponent {
  teamId: string | null = null;
  team?: Team;

  activatedRouteSubscription?: Subscription;
  teamServiceSubscription?: Subscription;

  constructor(private activatedRoute: ActivatedRoute,
    private teamService: TeamService,
    private sharedService: SharedService,
    private router: Router,
  ) {}

  ngOnInit() {
    this.activatedRouteSubscription = this.activatedRoute.paramMap.subscribe({
      next: (route) => {
        this.teamId = route.get('id');

        if(this.teamId) {
          this.teamServiceSubscription = this.teamService.getTeam(this.teamId).subscribe({
            next: (response) => {
              this.team = response;
            }
          })
        }
      }
    })
  }

  onDelete(teamId: string, event?: MouseEvent) {
    if(event) {
      event.stopPropagation();
    }

    this.teamServiceSubscription = this.teamService.deleteTeam(teamId).subscribe({
      next: (response) => {
        this.sharedService.showNotification(true, 'Success!', `Team ${response.name} has been deleted successfully!`);
        this.router.navigateByUrl(`/activity/teams/${response.activityRegistered.id}`);
      }
    })
  }

  ngOnDestroy() {
    this.activatedRouteSubscription?.unsubscribe();
    this.teamServiceSubscription?.unsubscribe();
  }
}
