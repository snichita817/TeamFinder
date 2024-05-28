import { Component } from '@angular/core';
import { AuthService } from '../../auth/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})
export class LandingPageComponent {

  constructor(private authService: AuthService,
    private router: Router
  ) {}

  onExplore() {
    if(this.authService.getUser()) {
      this.router.navigateByUrl('/activities')
    }
    else {
      this.router.navigateByUrl('/register')
    }
  }
}
