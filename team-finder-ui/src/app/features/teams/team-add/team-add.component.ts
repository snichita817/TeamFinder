import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../auth/services/auth.service';
import { TeamAddRequest } from '../models/team-add-request.model';
import { User } from '../../users/models/user.model';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { TeamService } from '../services/team.service';
import { SharedService } from 'src/app/shared/shared.service';

@Component({
  selector: 'app-team-add',
  templateUrl: './team-add.component.html',
  styleUrl: './team-add.component.css'
})
export class TeamAddComponent {
  activityId: string;
  submitted = false;

  teamForm: FormGroup = new FormGroup({});

  activatedRouteSubscription?: Subscription;
  teamServiceSubscription?: Subscription;

  constructor(private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private teamService: TeamService,
    private activatedRoute: ActivatedRoute,
    private sharedService: SharedService
  ) {
    this.activityId = '';
  }

  ngOnInit() {
    this.activatedRouteSubscription = this.activatedRoute.paramMap.subscribe({
      next: (params: any) => {
        this.activityId = params.get('activityId');
      }
    });
    this.initializeForm();
  }

  initializeForm() {
    this.teamForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      description: [''],
      isPrivate: [true],
      createdBy: [{value: this.authService.getUser()?.email, disabled: true}],
    })
  }

  onFormSubmit() {
    this.submitted = true;

    if(this.teamForm.valid) {
      const model: TeamAddRequest = {
        name: this.teamForm.get('name')?.value,
        description: this.teamForm.get('description')?.value,
        createdDate: new Date(),
        acceptedToActivity: false,
        isPrivate: this.teamForm.get('isPrivate')?.value,
        teamCaptainId: this.authService.getUser()?.id ?? '',
        activityRegistered: this.activityId,
        members: [this.authService.getUser()?.id ?? '']
      }
      this.teamServiceSubscription = this.teamService.addTeam(model).subscribe({
        next: (response) => {
          this.sharedService.showNotification(true, 'Congratulations!', `Team ${model.name} registered successfully!`);
          this.router.navigateByUrl(`/activities/get/${this.activityId}`);
        }
      })
      console.log(model);
    }

  }

  ngOnDestroy() {
    this.activatedRouteSubscription?.unsubscribe();
    this.teamServiceSubscription?.unsubscribe();
  }
}
