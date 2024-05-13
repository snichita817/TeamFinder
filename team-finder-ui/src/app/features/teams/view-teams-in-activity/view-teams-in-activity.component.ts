import { Component } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { Team } from '../models/team.model';
import { TeamService } from '../services/team.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-view-teams-in-activity',
  templateUrl: './view-teams-in-activity.component.html',
  styleUrl: './view-teams-in-activity.component.css'
})

export class ViewTeamsInActivityComponent {
  activityId: string = '';

  teams$?: Observable<Team[]>

  activatedRouteSubscription?: Subscription;

  constructor(private teamService: TeamService,
    private activatedRoute: ActivatedRoute
  ) {
    this.activatedRouteSubscription = this.activatedRoute.paramMap.subscribe({
      next: (route) => {
        this.activityId = route.get('activityId') ?? '';
      }
    });
  }

  ngOnInit(): void {
    this.teams$ = this.teamService.indexByActivity(this.activityId);

    this.teams$.subscribe({
      next: (teams) => {
        console.log('Teams:', teams);
      },
      error: (error) => {
        console.error('Error fetching teams:', error);
      }
    })
  }

  ngOnDestroy() {
    this.activatedRouteSubscription?.unsubscribe();
  }
}
