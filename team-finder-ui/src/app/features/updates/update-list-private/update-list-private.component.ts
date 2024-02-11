import { Component } from '@angular/core';
import { Update } from '../models/update.model';
import { Observable } from 'rxjs';
import { UpdateService } from '../services/update.service';

@Component({
  selector: 'app-update-list-private',
  templateUrl: './update-list-private.component.html',
  styleUrls: ['./update-list-private.component.css']
})
export class UpdateListPrivateComponent {
  updates$?: Observable<Update[]>;

  constructor(private updateService: UpdateService){
  }

  ngOnInit(): void {
    this.updates$ = this.updateService.indexUpdates();
  }
}
