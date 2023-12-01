import { Component } from '@angular/core';
import { ActivityAddRequest } from '../models/activity-add-request.model';

@Component({
  selector: 'app-activity-add',
  templateUrl: './activity-add.component.html',
  styleUrls: ['./activity-add.component.css']
})
export class ActivityAddComponent {
  model: ActivityAddRequest;

  constructor() {
    this.model = {
      title: '',
      shortDescription: '',
      longDescription: '',
      startDate: new Date(),
      endDate: new Date(),
      openRegistration: true,
      maxParticipants: 0,
      createdBy: ''
    }
  }

  onFormSubmit(){
    
  }
}
