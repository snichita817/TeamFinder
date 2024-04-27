import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ConfirmEmail } from '../models/confirm-email.model';
import { SharedService } from 'src/app/shared/shared.service';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.css'
})
export class ConfirmEmailComponent {
  success: boolean = true;

  activatedRouteSubscription?: Subscription;
  authServiceSubscription?: Subscription;

  constructor(private authService: AuthService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private sharedService: SharedService
  )
  {
  }

  ngOnInit(){
    this.activatedRouteSubscription = this.activatedRoute.queryParamMap.subscribe({
      next: (params) => {
        const confirmEmail: ConfirmEmail ={
          token: params.get('token') as string,
          email: params.get('email') as string,
        }

        this.authServiceSubscription = this.authService.confirmEmail(confirmEmail).subscribe({
          next: (response) => {
            this.authService.setUser({
              id: response.id,
              email: response.email,
              roles: response.roles
            });
            this.sharedService.showNotification(true, "Email confirmed successfully", "You have been successfully logged in to your account!")
            
            this.router.navigateByUrl('/')
          },
          error: error => {
            this.success = false;
            this.sharedService.showNotification(false, "Failed email confirmation", error.error)
          }
        })
      }
    })
  }

  resendEmailConfirmationLink()
  {

  }

  ngOnDestroy() {
    this.authServiceSubscription?.unsubscribe();
    this.activatedRouteSubscription?.unsubscribe();
  }
}
