import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { Activity } from '../models/activity.model';
import { ActivityService } from '../services/activity.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Team } from '../../teams/models/team.model';
import { SharedService } from 'src/app/shared/shared.service';
import { AuthService } from '../../auth/services/auth.service';
import { TeamService } from '../../teams/services/team.service';

@Component({
  selector: 'app-activity-get',
  templateUrl: './activity-get.component.html',
  styleUrls: ['./activity-get.component.css'],
})
export class ActivityGetComponent implements OnInit, OnDestroy {
  activityId: string | null = null;
  model?: Activity;
  team?: Team;
  teamId: string | null = null;

  routeSubscription?: Subscription;
  activityServiceSubscription?: Subscription;
  userTeamSubscription?: Subscription;
  teamServiceSubscription?: Subscription;

  dataSource: Team[] = [];

  constructor(
    private activityService: ActivityService,
    private route: ActivatedRoute,
    private router: Router,
    private sharedService: SharedService,
    private teamService: TeamService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.activityId = params.get('id');

        if (this.activityId) {
          this.activityServiceSubscription = this.activityService.getActivity(this.activityId).subscribe({
            next: (result) => {
              this.model = result;
              if (this.model.winnerResult != null) {
                this.dataSource = this.model.winnerResult.teams.map((team, index) => ({
                  ...team,
                  position: index + 1
                }));
                console.log(this.dataSource)
              }
            },
            error: (error) => {
              if (error.error) {
                this.sharedService.showNotification(false, 'Error!', error.error);
              }
              this.router.navigateByUrl('/activities');
            }
          });
        }
      }
    });

    if (this.activityId) {
      this.userTeamSubscription = this.teamService.getUserTeam(this.activityId).subscribe({
        next: (response) => {
          this.team = response;
          this.teamId = response.id;
        },
        error: () => {
          this.teamId = null;
        }
      });
    }
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.activityServiceSubscription?.unsubscribe();
    this.teamServiceSubscription?.unsubscribe();
    this.userTeamSubscription?.unsubscribe();
  }

  navigateToUpdate(id: string, event?: MouseEvent) {
    if (event) {
      event.stopPropagation();
    }

    this.router.navigate(['/updates/get', id]);
  }

  navigateToEditUpdate(id: string, event?: MouseEvent) {
    if (event) {
      event.stopPropagation();
    }

    this.router.navigate(['updates/edit', id]);
  }

  onDelete(id: string, event?: MouseEvent) {
    if (event) {
      event.stopPropagation();
    }

    this.activityServiceSubscription = this.activityService.deleteActivity(id).subscribe({
      next: (response) => {
        this.sharedService.showNotification(true, "Success!", `Activity ${response.title} deleted successfully!`);
        this.router.navigateByUrl('/activities');
      },
      error: (error) => {
        if (error.error) {
          this.sharedService.showNotification(false, 'Error!', error.error);
        }
        this.router.navigateByUrl('/activities');
      }
    });
  }

  canShow(): boolean {
    const user = this.authService.getUser();
    if (!user || !user.roles) {
      return false;
    }
    return (user.roles.includes("Organizer") && user.id === this.model?.createdBy.id) || user.roles.includes("Admin");
  }

  showRegisterButton(): boolean {
    if (this.model === undefined) {
      return false;
    }

    const currentDate = Date.now(); // date in milliseconds
    const startDate = new Date(this.model.startDate).getTime();

    if (startDate < currentDate) {
      return false;
    }

    return true;
  }

  pickWinners() {
    if(this.model) {
      const currentDate = Date.now();
      const endDate = new Date(this.model?.endDate).getTime();
      // if(currentDate < endDate) {
      //   this.sharedService.showNotification(false, "Error!", "You must wait until the end of activity to choose a winner!");
      //   return;
      // }
    }
    if (this.model?.winnerResult != null) {
      this.sharedService.showNotification(false, "Error!", "Winners are already picked!");
      return;
    }
    
    this.router.navigateByUrl(`/activity/${this.model?.id}/pick-winners`);
  }

  navigateToDeleteUpdate(id: string, event?: MouseEvent) {
    if (event) {
      event.stopPropagation();
    }
    this.router.navigate(['/updates/delete', id]);
  }
}