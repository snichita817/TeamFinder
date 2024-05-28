import { Component, OnDestroy, OnInit } from '@angular/core';
import { UserProfile } from '../models/user-profile.model';
import { UserService } from '../services/user.service';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { AddReviewDto } from '../../reviews/models/add-review.model';
import { Review } from '../../reviews/models/review.model';
import { ReviewService } from '../../reviews/services/review.service';
import { faStar } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit, OnDestroy {
  userId: string | null = null;
  model?: UserProfile;

  routeSubscription?: Subscription;
  userServiceSubscription?: Subscription;

  imageUrl: string = 'https://bootdey.com/img/Content/avatar/avatar7.png';

  reviews: Review[] = [];
  newReviewContent: string = '';
  newReviewRating: number = 5;
  faStar = faStar;

  constructor(private userService: UserService,
              private route: ActivatedRoute,
              private reviewService: ReviewService) {}

  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.userId = params.get('id');
        
        if(this.userId) {
          this.userServiceSubscription = this.userService.getUser(this.userId).subscribe({
            next: (response) => {
              this.model = {
                id: response.id,
                email: response.email,
                userName: response.userName,
                roles: response.roles,
                firstName: response.firstName ?? "Your",
                lastName: response.lastName ?? "Name",
                university: response.university ?? "",
                graduationYear: response.graduationYear,
                bio: response.bio ?? "No bio yet...",
                linkedinUrl: response.linkedinUrl ?? "",
                githubUrl: response.githubUrl ?? "",
                skills: response.skills ?? "",
                portfolioUrl: response.portfolioUrl ?? "",
                categories: response.categories ?? [],
                courseOfStudy: 0
              };
              if(response.profilePictureUrl) {
                this.imageUrl = `https://storage.googleapis.com/profile-picture-uploads/${response.profilePictureUrl}`;
              }

              this.loadReviews(this.model.id);
            }
          });
        }
      }
    });
  }

  loadReviews(organizerId: string): void {
    this.reviewService.getReviews(organizerId).subscribe(reviews => {
      this.reviews = reviews;
    });
  }

  postReview(): void {
    if (this.newReviewContent && this.model && this.model.id) {
      const newReview: AddReviewDto = {
        organizerId: this.model.id,
        content: this.newReviewContent,
        rating: this.newReviewRating
      };

      this.reviewService.postReview(newReview).subscribe(review => {
        this.ngOnInit();
      });
    }
  }

  setRating(rating: number): void {
    this.newReviewRating = rating;
  }

  getStarClass(star: number): string {
    return star <= this.newReviewRating ? 'checked' : '';
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.userServiceSubscription?.unsubscribe();
  }
}
