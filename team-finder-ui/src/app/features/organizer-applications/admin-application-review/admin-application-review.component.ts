import { Component, OnInit } from '@angular/core';
import { OrganizerApplicationService } from '../services/organizer-application.service';
import { OrganizerApplication } from '../models/organizer-application-req.model';

@Component({
  selector: 'app-admin-application-review',
  templateUrl: './admin-application-review.component.html',
  styleUrls: ['./admin-application-review.component.css']
})
export class AdminApplicationReviewComponent implements OnInit {
  applications: OrganizerApplication[] = [];
  expandedApplications: Set<string> = new Set();

  constructor(private organizerApplicationService: OrganizerApplicationService) {}

  ngOnInit(): void {
    this.organizerApplicationService.getApplications().subscribe((data: OrganizerApplication[]) => {
      this.applications = data;
    }, error => {
      console.error('Error fetching applications:', error);
    });
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

  approve(id: string) {
    this.organizerApplicationService.approveApplication(id).subscribe(() => {
      alert('Application approved successfully.');
      this.ngOnInit(); // Refresh the list
    }, error => {
      alert('An error occurred while approving the application.');
    });
  }

  reject(id: string) {
    this.organizerApplicationService.rejectApplication(id).subscribe(() => {
      alert('Application rejected successfully.');
      this.ngOnInit(); // Refresh the list
    }, error => {
      alert('An error occurred while rejecting the application.');
    });
  }
}
