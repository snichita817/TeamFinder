import { Component } from '@angular/core';
import { UserProfile } from '../models/user-profile.model';
import { Observable, Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { UserEditRequest } from '../models/user-edit-request.model';
import { CategoryService } from '../../categories/services/category.service';
import { Category } from '../../categories/models/category.model';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrl: './user-edit.component.css'
})
export class UserEditComponent {
  id: string | null = null;
  model?: UserProfile;
  categories$?: Observable<Category[]>
  selectedCategories?: string[];

  routeSubscription?: Subscription;
  getUserSubscription?: Subscription;
  editUserSubscription?: Subscription;

  constructor(private route: ActivatedRoute,
    private userService: UserService,
    private categoryService: CategoryService,
    private router: Router) {}

  ngOnInit(): void {
    this.categories$ = this.categoryService.indexCategories();

    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if(this.id) {
          this.getUserSubscription = this.userService.getUser(this.id).subscribe({
            next: (result) => {
              this.model = result;
              this.selectedCategories = result.categories.map(x => x.id)
            }
          })
        }
      }
    })
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.getUserSubscription?.unsubscribe();
    this.editUserSubscription?.unsubscribe();
  }

  onFormSubmit(): void {
    const editUserRequest: UserEditRequest = {
      email: this.model?.email ?? "",
      userName: this.model?.userName ?? "",
      roles: this.model?.roles ?? [],
      firstName: this.model?.firstName ?? "",
      lastName: this.model?.lastName ?? "",
      university: this.model?.university ?? "",
      categories: this.selectedCategories ?? [],
      graduationYear: this.model?.graduationYear ?? new Date().getFullYear(),
      bio: this.model?.bio ?? "No bio yet...",
      linkedinUrl: this.model?.linkedinUrl ?? "",
      githubUrl: this.model?.githubUrl ?? "",
      skills: this.model?.skills ?? "",
      portfolioUrl: this.model?.portfolioUrl ?? ""
    }
    if(this.id) {
      console.log(this.id)
      this.editUserSubscription = this.userService.updateUser(this.id, editUserRequest).subscribe({
        next: (response) => {
          this.router.navigateByUrl('');
        }
      })
    }
  }

}
