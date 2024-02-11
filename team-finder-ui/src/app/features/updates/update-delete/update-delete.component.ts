import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { UpdateService } from '../services/update.service';

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
    private router: Router) {}
  
  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if(this.id) {
          this.deleteUpdateSubscription = this.updateService.deleteUpdate(this.id).subscribe({
            next: (response) => {
              this.router.navigateByUrl('');
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
