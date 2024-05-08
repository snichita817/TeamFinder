import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { SharedService } from 'src/app/shared/shared.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-send-email',
  templateUrl: './send-email.component.html',
  styleUrl: './send-email.component.css'
})
export class SendEmailComponent {
  emailForm: FormGroup = new FormGroup([]);
  submitted = false;
  mode: string | undefined;
  errorMessages: string[] = [];

  authServiceSubscription?: Subscription;

  constructor(private authService: AuthService,
    private sharedService: SharedService,
    private formBuilder: FormBuilder,
    private router: Router,
    private activatedRoute: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    // Forward path if already logged in
    if(this.authService.getUser() != undefined) {
      this.router.navigateByUrl('/');
    } else {
      const mode = this.activatedRoute.snapshot.paramMap.get('mode');

      if(mode) {
        this.mode = mode;
        console.log(this.mode)
        this.initializeForm();
      }
    }
  }

  initializeForm() {
    this.emailForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.pattern('^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$')]],
    })
  }

  sendEmail() {
    this.submitted = true;
    this.errorMessages = [];

    if(this.emailForm.valid && this.mode) {
      if(this.mode.includes('resend-email-confirmation-link')) {
        this.authServiceSubscription = this.authService.resendEmailConfirmationLink(this.emailForm.get('email')?.value).subscribe({
          next: (response: any) => {
            this.sharedService.showNotification(true, response.value.title, response.value.message);
            this.router.navigateByUrl('/login')
          }, error: error => {
            if(error.error.errors) {
              this.errorMessages = error.error.errors; // this are the errors coming from Backend
            } else {
              this.errorMessages.push(error.error);
            }
          }
        })
      }
    }
  }

  cancel() {
    this.router.navigateByUrl('/login');
  }
}
