import { Component, OnInit } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { Activity } from '../models/activity.model';
import { ActivityService } from '../services/activity.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Update } from '../../updates/models/update.model';
import { SharedService } from 'src/app/shared/shared.service';
import { AuthService } from '../../auth/services/auth.service';
import { TeamService } from '../../teams/services/team.service';
import { Team } from '../../teams/models/team.model';

@Component({
  selector: 'app-activity-get',
  templateUrl: './activity-get.component.html',
  styleUrls: ['./activity-get.component.css']
})
export class ActivityGetComponent implements OnInit {
  activityId: string | null = null;
  model?: Activity;
  team?: Team;

  routeSubscription?: Subscription;
  activityServiceSubscription?: Subscription;
  teamServiceSubscription?: Subscription;

  constructor(private activityService: ActivityService,
    private route:ActivatedRoute,
    private router: Router,
    private sharedService: SharedService,
    private teamService: TeamService,
    private authService: AuthService) {
  }

  ngOnInit(): void {
    // if(!this.authService.getUser()) {
    //   this.sharedService.showNotification(false, "Error!", "You should login first before accessing this page!");
    //   this.router.navigateByUrl('/login');
    // }
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.activityId = params.get('id');

        if(this.activityId){
          this.activityServiceSubscription = this.activityService.getActivity(this.activityId).subscribe({
            next: (result) => {
              this.model = result;
            },
            error: (error) => {
              if(error.error) {
                this.sharedService.showNotification(false, 'Error!', error.error);
              }
              this.router.navigateByUrl('/activities')
            }
          });
        }
      }
    });
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.activityServiceSubscription?.unsubscribe();
    this.teamServiceSubscription?.unsubscribe();
  }

  navigateToUpdate(id: string, event?:MouseEvent) {
    if(event) {
      event.stopPropagation();
    }

    this.router.navigate(['/updates/get', id]);
  }

  navigateToEditUpdate(id: string, event?:MouseEvent) { 
    if(event) {
      event.stopPropagation();
    }
    
    this.router.navigate(['updates/edit', id]);
  }

  onDelete(id: string, event?:MouseEvent) {
    if(event) {
      event.stopPropagation();
    }

    this.activityServiceSubscription = this.activityService.deleteActivity(id).subscribe({
      next: (response) => {
        this.sharedService.showNotification(true, "Succcess!", `Activity ${response.title} deleted successfully!`);
        this.router.navigateByUrl('/activities')
      },
      error: (error) => {
        if(error.error) {
          this.sharedService.showNotification(false, 'Error!', error.error);
        }
        this.router.navigateByUrl('/activities')
      }
    })
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

 
  navigateToDeleteUpdate(id: string, event?:MouseEvent) { 
    if(event) {
      event.stopPropagation();
    }
    this.router.navigate(['/updates/delete', id]);
  }
}
