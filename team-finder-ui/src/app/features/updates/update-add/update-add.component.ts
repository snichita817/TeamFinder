import { Component } from '@angular/core';
import { AddUpdateRequest } from '../models/update-add-request.model';
import { Subscription } from 'rxjs';
import { UpdateService } from '../services/update.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-update-add',
  templateUrl: './update-add.component.html',
  styleUrls: ['./update-add.component.css']
})
export class UpdateAddComponent {
  model: AddUpdateRequest;

  private addUpdateSubscription?: Subscription;

  constructor(private updateService: UpdateService,
    private router: Router){
    this.model = {
      title: '',
      text: '',
      date: new Date()
    }
  }

  onFormSubmit()
  {
    this.addUpdateSubscription = this.updateService.addUpdate(this.model)
    .subscribe({
      next: (response) => {
        this.router.navigateByUrl('');
      }
    })
  }
}
