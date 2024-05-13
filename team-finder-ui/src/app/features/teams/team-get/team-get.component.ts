import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TeamService } from '../services/team.service';
import { Subscription } from 'rxjs';
import { Team } from '../models/team.model';

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
    private teamService: TeamService
  ) {}

  ngOnInit() {
    this.activatedRouteSubscription = this.activatedRoute.paramMap.subscribe({
      next: (route) => {
        this.teamId = route.get('id');

        if(this.teamId) {
          this.teamServiceSubscription = this.teamService.getTeam(this.teamId).subscribe({
            next: (response) => {
              this.team = response;
              console.log(this.team);
            }
          })
        }
      }
    })
  }

  ngOnDestroy() {
    this.activatedRouteSubscription?.unsubscribe();
    this.teamServiceSubscription?.unsubscribe();
  }
}
