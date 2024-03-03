import { Component } from '@angular/core';
import { EditUpdateRequest } from '../models/update-edit-request.model';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { UpdateService } from '../services/update.service';

@Component({
  selector: 'app-update-edit',
  templateUrl: './update-edit.component.html',
  styleUrls: ['./update-edit.component.css']
})
export class UpdateEditComponent {
  model?: EditUpdateRequest; 
  
  routeSubscription?: Subscription;
  updateServiceSubscription?: Subscription;
  editUpdateServiceSubscription?: Subscription;

  id: string | null = "";

  constructor(private route : ActivatedRoute,
    private updateService: UpdateService,
    private router: Router) {}

  ngOnInit() {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if(this.id) {
          this.updateServiceSubscription = this.updateService.getUpdate(this.id).subscribe({
            next: (response) => {
              this.model = response;
            }
          })
        }
      }
    })
  }

  ngOnDestroy() {
    this.routeSubscription?.unsubscribe();
    this.updateServiceSubscription?.unsubscribe();
  }

  onFormSubmit(): void {
    if(this.model && this.id)
    {
      var editedUpdate: EditUpdateRequest = {
        title: this.model.title,
        text: this.model.text,
        date: new Date,
        activityId: this.model.activityId
      }
    }
    this.editUpdateServiceSubscription = this.updateService.editUpdate(this.id, this.model)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl(`/activities/get/${this.model?.activityId}`);
        }
      })
  }
}
