import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { Team } from '../models/team.model';
import { TeamService } from '../services/team.service';
import { User } from '../../users/models/user.model';

@Component({
  selector: 'app-team-get-all',
  templateUrl: './team-get-all.component.html',
  styleUrl: './team-get-all.component.css'
})
export class TeamGetAllComponent {
  teams?: Team[];

  teamServiceSubscription?: Subscription;

  constructor(private teamService: TeamService) {}

  ngOnInit() {
    this.teamServiceSubscription = this.teamService.getAllTeams().subscribe({
      next: (response) => {
        this.teams = response;

        console.log(this.teams);
      }
    })
  }

  ngOnDestroy() {
    this.teamServiceSubscription?.unsubscribe();
  }

  getCaptainName(members: User[], captainId: string) {
    const captain = members.find(member => member.id === captainId);
    return captain ? captain.userName : "Username not found"
  }
}
