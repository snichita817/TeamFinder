import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TeamService } from '../services/team.service';
import { StorageService } from 'src/app/shared/storage/storage.service';
import { Subscription } from 'rxjs';
import { Team } from '../models/team.model';
import { SharedService } from 'src/app/shared/shared.service';

@Component({
  selector: 'app-team-get',
  templateUrl: './team-get.component.html',
  styleUrls: ['./team-get.component.css']
})
export class TeamGetComponent implements OnInit, OnDestroy {
  teamId: string | null = null;
  team?: Team;
  fileToUpload: File | null = null;

  activatedRouteSubscription?: Subscription;
  teamServiceSubscription?: Subscription;

  submissionUrl: string | null = null;

  constructor(
    private activatedRoute: ActivatedRoute,
    private teamService: TeamService,
    private storageService: StorageService,
    private sharedService: SharedService,
    private router: Router
  ) {}

  ngOnInit() {
    this.activatedRouteSubscription = this.activatedRoute.paramMap.subscribe({
      next: (route) => {
        this.teamId = route.get('id');

        if(this.teamId) {
          this.teamServiceSubscription = this.teamService.getTeam(this.teamId).subscribe({
            next: (response) => {
              this.team = response;

              if(response.submissionUrl) {
                console.log(response.submissionUrl)
                this.submissionUrl = `https://storage.googleapis.com/team-submissions/${response.submissionUrl}`
              }
            }
          });
        }
      }
    });
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
    });
  }

  handleFileInput(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.fileToUpload = input.files[0];
    }
  }

  uploadFileToActivity() {
    if (!this.fileToUpload) {
      this.sharedService.showNotification(false, 'Error!', 'Please select a file to upload.');
      return;
    }

    const formData: FormData = new FormData();
    var str = new Date().setSeconds(0,0);
    var dt = new Date(str).toISOString();
    formData.append('file', this.fileToUpload, 'team-'+this.teamId+'-'+dt);

    if(this.teamId) {
      this.storageService.addFile(formData, 'team-submissions').subscribe({
        next: (fileName: string) => {
          this.sharedService.showNotification(true, 'Success!', `File ${this.fileToUpload?.name} has been uploaded successfully!`);
  
          this.teamServiceSubscription = this.teamService.changeSubmissionUrl(this.teamId ?? '', fileName).subscribe({
            next: (response) => {
              this.ngOnInit(); // refresh page
            }
          })
        },
        error: (error) => {
          this.sharedService.showNotification(false, 'Error!', 'File upload failed. Please try again.');
        }
      });
    }
    
  }

  ngOnDestroy() {
    this.activatedRouteSubscription?.unsubscribe();
    this.teamServiceSubscription?.unsubscribe();
  }
}
