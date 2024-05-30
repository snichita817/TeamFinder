import { Component, OnInit, OnDestroy } from '@angular/core';
import { UserProfile } from '../models/user-profile.model';
import { Observable, Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { UserEditRequest } from '../models/user-edit-request.model';
import { CategoryService } from '../../categories/services/category.service';
import { Category } from '../../categories/models/category.model';
import { StorageService } from 'src/app/shared/storage/storage.service';
import { ProcessCvService } from 'src/app/shared/process-cv/process-cv.service';
import { SharedService } from 'src/app/shared/shared.service';
import { CvInfo } from '../models/cv-info.model';
import { AuthService } from '../../auth/services/auth.service';

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
  cvServiceSubscription?: Subscription;

  cv: File | null = null;
  selectedFile: File | null = null;
  imageUrl: string = 'https://bootdey.com/img/Content/avatar/avatar7.png';
  isProcessing: boolean = false; // Spinner control variable

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private categoryService: CategoryService,
    private router: Router,
    private storageService: StorageService,
    private sharedService: SharedService,
    private cvService: ProcessCvService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.categories$ = this.categoryService.indexCategories();

    this.routeSubscription = this.route.paramMap.subscribe(params => {
      this.id = params.get('id');

      if (this.id) {
        if(this.isUserCorrect(this.id) == false) {
          this.sharedService.showNotification(false, "Error", "You have no access to edit this profile!")
          this.router.navigateByUrl('')
          return;
        } 
        this.getUserSubscription = this.userService.getUser(this.id).subscribe(result => {
          this.model = result;
          if (this.model.profilePictureUrl) {
            this.imageUrl = `https://storage.googleapis.com/profile-picture-uploads/${this.model.profilePictureUrl}`;
          }
          this.selectedCategories = result.categories.map(x => x.id);
        });
      }
    });
  }

  isUserCorrect(id: string): boolean {
    const user = this.authService.getUser()
    if(user == undefined) {
      return false;
    }
    return user.id === id;
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.getUserSubscription?.unsubscribe();
    this.editUserSubscription?.unsubscribe();
    this.storageServiceSubscription?.unsubscribe();
    this.cvServiceSubscription?.unsubscribe();
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

  handleFileInput(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.cv = input.files[0];
    }
  }

  uploadCv() {
    if (!this.cv) {
      this.sharedService.showNotification(false, 'Error!', 'Please select a file to upload.');
      return;
    }

    this.isProcessing = true; // Show spinner

    const formData: FormData = new FormData();
    formData.append('file', this.cv, this.cv.name);

    this.cvServiceSubscription = this.cvService.uploadCv(formData).subscribe({
      next: (cvInfo: CvInfo) => {
        this.updateModelWithCvInfo(cvInfo);
        this.sharedService.showNotification(true, 'Success!', 'File uploaded and processed successfully.');
        this.isProcessing = false; // Hide spinner
      },
      error: (err) => {
        console.error('Error uploading file', err);
        this.sharedService.showNotification(false, 'Error!', 'Error uploading file.');
        this.isProcessing = false; // Hide spinner
      }
    });
  }

  private updateModelWithCvInfo(cvInfo: CvInfo) {
    if (!this.model) return;

    this.model.firstName = cvInfo.firstName;
    this.model.lastName = cvInfo.lastName;
    this.model.university = cvInfo.university;
    this.model.graduationYear = cvInfo.graduationYear;
    this.model.skills = cvInfo.skills;
    this.model.linkedinUrl = cvInfo.linkedinUrl;
    this.model.githubUrl = cvInfo.githubUrl;
    this.model.portfolioUrl = cvInfo.portfolioUrl;
    this.model.bio = cvInfo.bio;
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
        this.sharedService.showNotification(false, "Unauthorized!", "You do not have access to edit this page!");
        this.router.navigateByUrl('');
      }
    });
  }
}
