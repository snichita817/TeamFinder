import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { UpdateService } from '../services/update.service';
import { SharedService } from 'src/app/shared/shared.service';

@Component({
  selector: 'app-update-delete',
  templateUrl: './update-delete.component.html',
  styleUrls: ['./update-delete.component.css']
})
export class UpdateDeleteComponent {
  id: string | null = null;

  routeSubscription?: Subscription;
  deleteUpdateSubscription?: Subscription;

  constructor(private route: ActivatedRoute,
    private updateService: UpdateService,
    private router: Router,
    private sharedService: SharedService) {}
  
  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if(this.id) {
          this.deleteUpdateSubscription = this.updateService.deleteUpdate(this.id).subscribe({
            next: (response) => {
              this.router.navigateByUrl('');
            },
            error: (error) => {
              if(error.error) {
                this.sharedService.showNotification(false, 'Error!', error.error);
              }
            }
          })
        }
      }
    })
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.deleteUpdateSubscription?.unsubscribe();
  }
}
