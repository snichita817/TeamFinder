import { Component } from '@angular/core';
import { AddUpdateRequest } from '../models/update-add-request.model';
import { Subscription } from 'rxjs';
import { UpdateService } from '../services/update.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-update-add',
  templateUrl: './update-add.component.html',
  styleUrls: ['./update-add.component.css']
})
export class UpdateAddComponent {
  model: AddUpdateRequest;
  activityId: string | null = null;

  private routeSubscription?: Subscription;
  private addUpdateSubscription?: Subscription;

  constructor(private updateService: UpdateService,
    private route: ActivatedRoute,
    private router: Router){
      this.model = {
        title: '',
        text: '',
        date: new Date(),
        activityId: '',
      }
  }

  onFormSubmit()
  {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.activityId = params.get('activityId')

        if(this.activityId) {
          this.model.activityId = this.activityId;
          this.addUpdateSubscription = this.updateService.addUpdate(this.model)
          .subscribe({
            next: (response) => {
              this.router.navigateByUrl(`activities/get/${this.activityId}`,);
            }
          })
        }
      }
    })
  }

  ngOnDestroy() {
    this.routeSubscription?.unsubscribe();
    this.addUpdateSubscription?.unsubscribe();
  }
}
