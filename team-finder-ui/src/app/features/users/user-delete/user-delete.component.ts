import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-user-delete',
  templateUrl: './user-delete.component.html',
  styleUrl: './user-delete.component.css'
})
export class UserDeleteComponent {
  id: string | null | undefined;

  routeSubscription?: Subscription;
  userServiceSubscription?: Subscription;

  constructor(private route: ActivatedRoute,
    private userService: UserService,
    private router: Router) {}

  ngOnInit() {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if(this.id) {
          this.userServiceSubscription = this.userService.deleteUser(this.id).subscribe({
            next: (response) => {
              this.router.navigateByUrl('/');
            },
          })
        }
      }
    })
  }

  ngOnDestroy() {
    this.routeSubscription?.unsubscribe();
    this.userServiceSubscription?.unsubscribe();
  }
}
