import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Team } from '../models/team.model';
import { TeamService } from '../services/team.service';
import { TeamEditRequest } from '../models/team-edit-request.model';
import { User } from '../../users/models/user.model';
import { SharedService } from 'src/app/shared/shared.service';

@Component({
  selector: 'app-team-edit',
  templateUrl: './team-edit.component.html',
  styleUrls: ['./team-edit.component.css']
})
export class TeamEditComponent implements OnInit, OnDestroy {
  teamId: string | null = null;
  team?: Team;
  teamForm: FormGroup = new FormGroup({});
  activatedRouteSubscription?: Subscription;
  teamServiceSubscription?: Subscription;

  constructor(
    private activatedRoute: ActivatedRoute,
    private formBuilder: FormBuilder,
    private teamService: TeamService,
    private router: Router,
    private sharedService: SharedService
  ) {}

  ngOnInit() {
    this.activatedRouteSubscription = this.activatedRoute.paramMap.subscribe({
      next: (route) => {
        this.teamId = route.get('id');
        if (this.teamId) {
          this.teamServiceSubscription = this.teamService.getTeam(this.teamId).subscribe({
            next: (response) => {
              this.team = response;
              this.initializeForm();
            }
          });
        }
      }
    });
  }

  initializeForm() {
    if (this.team) {
      this.teamForm = this.formBuilder.group({
        name: [this.team.name],
        description: [this.team.description],
        isPrivate: [this.team.isPrivate],
        teamCaptainId: [this.team.teamCaptainId, [Validators.required]]
      });
      console.log(this.team);
    }
  }

  saveTeam() {
    if (this.teamForm.valid && this.team) {
      const updatedTeam: TeamEditRequest = {
        name: this.teamForm.get('name')?.value,
        description: this.teamForm.get('description')?.value,
        isPrivate: this.teamForm.get('isPrivate')?.value,
        teamCaptainId: this.teamForm.get('teamCaptainId')?.value,
        members: this.team.members.map(mem => mem.id)
      };

      console.log(updatedTeam);
      this.teamServiceSubscription = this.teamService.editTeam(updatedTeam, this.team.id).subscribe({
        next: (response) => {
          this.router.navigateByUrl(`/team/view/${this.teamId}`);
        },
        error: (error) => {
          console.log(error)
          if(error.error) {
            this.sharedService.showNotification(false, 'Error!', error.error);
          }
        }
      });
    }
  }

  ngOnDestroy() {
    this.activatedRouteSubscription?.unsubscribe();
    this.teamServiceSubscription?.unsubscribe();
  }
}