import { Component } from '@angular/core';
import { Update } from '../models/update.model';
import { Subscription } from 'rxjs';
import { UpdateService } from '../services/update.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-update-get',
  templateUrl: './update-get.component.html',
  styleUrls: ['./update-get.component.css']
})
export class UpdateGetComponent {
  updateId: string | null = null;
  model?: Update;

  routeSubscription?: Subscription;
  getUpdateSubscription?: Subscription;

  constructor(private updateService: UpdateService,
    private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.updateId = params.get('id');

        if(this.updateId) {
          this.getUpdateSubscription = this.updateService.getUpdate(this.updateId).subscribe({
            next: (result) => {
              this.model = result;
            }
          });
        }
      }
    });
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.getUpdateSubscription?.unsubscribe();
  }

  toggleDetails(): void {}
}
