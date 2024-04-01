import { Component } from '@angular/core';
import { UserProfile } from '../models/user-profile.model';
import { UserService } from '../services/user.service';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent {
  userId: string | null = null;
  model?: UserProfile;

  routeSubscription?: Subscription;
  userServiceSubscription?: Subscription;

  constructor(private userService: UserService,
    private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.userId = params.get('id');
        
        if(this.userId) {
          this.userServiceSubscription = this.userService.getUser(this.userId).subscribe({
            next: (response) => {
              console.log(response);
              this.model = {
                id: response.id,
                email: response.email,
                userName: response.userName,
                roles: response.roles,
                firstName: response.firstName ?? "",
                lastName: response.lastName ?? "",
                university: response.university ?? "",
                categories: response.categories ?? null,
                graduationYear: response.graduationYear ?? Date.now,
                bio: response.bio ?? "No bio yet...",
                linkedinUrl: response.linkedinUrl ?? "",
                githubUrl: response.githubUrl ?? "",
                skills: response.skills ?? "",
                interests: response.interests ?? "",
                portfolioUrl: response.portfolioUrl ?? ""
              };
              console.log(this.model);
            }
          });
        }
      }
    })
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.userServiceSubscription?.unsubscribe();
  }
}
