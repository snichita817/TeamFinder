import { Component } from '@angular/core';
import { OrganizerApplicationService } from '../services/organizer-application.service';
import { SharedService } from 'src/app/shared/shared.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { OrganizerApplication } from '../models/organizer-application-req.model';
import { AuthService } from '../../auth/services/auth.service';

@Component({
  selector: 'app-application-status',
  templateUrl: './application-status.component.html',
  styleUrl: './application-status.component.css'
})
export class ApplicationStatusComponent {
  applications: OrganizerApplication[] = [];
  expandedApplications: Set<string> = new Set();

  applicationServiceSubscription?: Subscription;

  constructor(private applicationService: OrganizerApplicationService,
    private sharedService: SharedService,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit() {
    if(this.authService.getUser() == undefined) {
      this.sharedService.showNotification(false, "Error!", "Please log in before viewing this page!");
      this.router.navigateByUrl('login')
    }

    this.applicationServiceSubscription = this.applicationService.getUserApplications().subscribe({
      next: (response) => {
        this.applications = response
      }
    })
  }

  toggleReason(applicationId: string) {
    if (this.expandedApplications.has(applicationId)) {
      this.expandedApplications.delete(applicationId);
    } else {
      this.expandedApplications.add(applicationId);
    }
  }

  isReasonExpanded(applicationId: string): boolean {
    return this.expandedApplications.has(applicationId);
  }

  ngOnDestroy() {
    this.applicationServiceSubscription?.unsubscribe();
  }

}