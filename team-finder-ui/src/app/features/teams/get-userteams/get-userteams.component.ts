import { Component } from '@angular/core';
import { Team } from '../models/team.model';
import { Subscription } from 'rxjs';
import { TeamService } from '../services/team.service';
import { AuthService } from '../../auth/services/auth.service';
import { SharedService } from 'src/app/shared/shared.service';

@Component({
  selector: 'app-get-userteams',
  templateUrl: './get-userteams.component.html',
  styleUrl: './get-userteams.component.css'
})
export class GetUserteamsComponent {
  teams: Team[] = [];

  teamServiceSubscription?: Subscription;

  constructor(private teamService: TeamService,
    private authService: AuthService,
    private sharedService: SharedService
  ) {}

  ngOnInit() {
    if(this.authService.getUser() == undefined) {
      this.sharedService.showNotification(false, "Error!", "Please log in before accessing this page!")
    }

    this.teamServiceSubscription = this.teamService.getUserTeams().subscribe({
      next: (response) => {
        this.teams = response;
        console.log(this.teams)
      }
    })
  }

  ngOnDelete() {
    this.teamServiceSubscription?.unsubscribe()
  }
}
