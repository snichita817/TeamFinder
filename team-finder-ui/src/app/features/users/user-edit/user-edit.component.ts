import { Component, OnInit, OnDestroy } from '@angular/core';
import { UserProfile } from '../models/user-profile.model';
import { Observable, Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { UserEditRequest } from '../models/user-edit-request.model';
import { CategoryService } from '../../categories/services/category.service';
import { Category } from '../../categories/models/category.model';
import { StorageService } from 'src/app/shared/storage/storage.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit, OnDestroy {
  id: string | null = null;
  model?: UserProfile;
  categories$?: Observable<Category[]>;
  selectedCategories?: string[];

  routeSubscription?: Subscription;
  getUserSubscription?: Subscription;
  editUserSubscription?: Subscription;
  storageServiceSubscription?: Subscription;

  selectedFile: File | null = null;
  imageUrl: string = 'https://bootdey.com/img/Content/avatar/avatar7.png';

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private categoryService: CategoryService,
    private router: Router,
    private storageService: StorageService
  ) {}

  ngOnInit(): void {
    this.categories$ = this.categoryService.indexCategories();

    this.routeSubscription = this.route.paramMap.subscribe(params => {
      this.id = params.get('id');
      if (this.id) {
        this.getUserSubscription = this.userService.getUser(this.id).subscribe(result => {
          this.model = result;
          console.log(this.model)

          if (this.model.profilePictureUrl) {
            this.imageUrl = `https://storage.googleapis.com/profile-picture-uploads/${this.model.profilePictureUrl}`;
          }
          this.selectedCategories = result.categories.map(x => x.id);
        });
      }
    });
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.getUserSubscription?.unsubscribe();
    this.editUserSubscription?.unsubscribe();
    this.storageServiceSubscription?.unsubscribe();
  }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
    if (this.selectedFile) {
      const reader = new FileReader();
      reader.onload = () => {
        this.imageUrl = reader.result as string;
      };
      reader.readAsDataURL(this.selectedFile);
    }
  }

  onFormSubmit(): void {
    if (!this.id || !this.model) return;

    const editUserRequest: UserEditRequest = {
      email: this.model.email,
      userName: this.model.userName,
      roles: this.model.roles,
      firstName: this.model.firstName,
      lastName: this.model.lastName,
      university: this.model.university,
      categories: this.selectedCategories ?? [],
      graduationYear: this.model.graduationYear,
      bio: this.model.bio,
      linkedinUrl: this.model.linkedinUrl,
      githubUrl: this.model.githubUrl,
      skills: this.model.skills,
      portfolioUrl: this.model.portfolioUrl,
      profilePictureUrl: this.model.profilePictureUrl // Ensure this is set correctly
    };

    if (this.selectedFile) {
      const formData = new FormData();
      var str = new Date().setSeconds(0,0);
      var dt = new Date(str).toISOString();
      formData.append('file', this.selectedFile, 'profile-picture-'+this.id+'-'+dt);

      this.storageServiceSubscription = this.storageService.addFile(formData, "profile-picture-uploads").subscribe({
        next: (fileName: string) => {
          editUserRequest.profilePictureUrl = fileName;
          this.updateUser(editUserRequest);
        },
        error: (err) => {
          console.error('Error uploading file', err);
        }
      });
    } else {
      this.updateUser(editUserRequest);
    }
  }

  private updateUser(editUserRequest: UserEditRequest): void {
    this.editUserSubscription = this.userService.updateUser(this.id!, editUserRequest).subscribe({
      next: () => {
        this.router.navigateByUrl(`/user/${this.id}`);
      },
      error: (err) => {
        console.error('Error updating user', err);
      }
    });
  }
}
