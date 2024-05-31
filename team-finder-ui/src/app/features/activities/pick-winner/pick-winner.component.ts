import { Component, OnDestroy, OnInit } from '@angular/core';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../../auth/services/auth.service';
import { SharedService } from 'src/app/shared/shared.service';
import { Team } from '../../teams/models/team.model';
import { TeamService } from '../../teams/services/team.service';
import { PickWinnerAdd } from '../models/pick-winner-add.model';
import { ActivityService } from '../services/activity.service';

@Component({
  selector: 'app-pick-winner',
  templateUrl: './pick-winner.component.html',
  styleUrls: ['./pick-winner.component.css']
})
export class PickWinnerComponent implements OnInit, OnDestroy {
  activityId: string | null = null;
  teams: Team[] = [];

  teamServiceSubscription?: Subscription;

  allTeams: Team[] = [];
  winners: Team[] = [];

  pickWinnerAdd: PickWinnerAdd | null = null;

  constructor(
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private sharedService: SharedService,
    private teamService: TeamService,
    private activityService: ActivityService,
    private router: Router
  ) {}

  ngOnInit() {
    const user = this.authService.getUser();
    if(user == undefined) {
      this.sharedService.showNotification(false, "Error!", "You must login before accessing this page!");
      this.router.navigateByUrl('/login');
    }
    if(!user?.roles.includes("Organizer") && !user?.roles.includes("Admin")) {
      this.sharedService.showNotification(false, "Error!", "You are not authorized to view this page!");
      this.router.navigateByUrl('');
    }
    this.activityId = this.activatedRoute.snapshot.paramMap.get('activityId');
    if (this.activityId) {
      this.teamServiceSubscription = this.teamService.indexByActivity(this.activityId).subscribe({
        next: (response) => {
          this.teams = response;
          this.allTeams = [...this.teams.filter(t => t.acceptedToActivity === "Accepted")];
          if(this.allTeams.length === 0) {
            this.sharedService.showNotification(false, "Error", "There are no teams to select for this activity!");
            this.router.navigateByUrl(`/activities/get/${this.activityId}`)
          }
        },
        error: (err) => console.error(err)
      });
    }
  }

  drop(event: CdkDragDrop<Team[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    }
  }
  confirmFinalizeWinners() {
    if (confirm("Are you sure you want to finalize the winners? This cannot be undone!")) {
        this.finalizeWinners();
    }
  }

  finalizeWinners() {
    if (!this.activityId) {
      alert('Activity ID is missing!');
      return;
    }

    const winnerResultDto: PickWinnerAdd = {
      activityId: this.activityId,
      teamIds: this.winners.map(team => team.id)
    };
    console.log(winnerResultDto)

    this.activityService.createWinnerResult(winnerResultDto).subscribe({
      next: (response) => {
        this.sharedService.showNotification(true, "Success!", "Winning teams have been submitted!")
      },
      error: (error) => {
        if(error.error) {
          this.sharedService.showNotification(false, 'Error!', error.error);
        }
      }
    });
  }

  ngOnDestroy() {
    this.teamServiceSubscription?.unsubscribe();
  }
}