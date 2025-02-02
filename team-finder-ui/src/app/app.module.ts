import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './core/components/navbar/navbar.component';
import { FooterComponent } from './core/components/footer/footer.component';
import { ActivityListComponent } from './features/activities/activity-list/activity-list.component';
import { ActivityAddComponent } from './features/activities/activity-add/activity-add.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ActivityListPrivateComponent } from './features/activities/activity-list-private/activity-list-private.component';
import { ActivityEditComponent } from './features/activities/activity-edit/activity-edit.component';
import { ActivityDeleteComponent } from './features/activities/activity-delete/activity-delete.component';
import { ActivityGetComponent } from './features/activities/activity-get/activity-get.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UpdateAddComponent } from './features/updates/update-add/update-add.component';
import { UpdateListPrivateComponent } from './features/updates/update-list-private/update-list-private.component';
import { UpdateDeleteComponent } from './features/updates/update-delete/update-delete.component';
import { UpdateGetComponent } from './features/updates/update-get/update-get.component';
import { UpdateEditComponent } from './features/updates/update-edit/update-edit.component';
import { CategoriesListPrivateComponent } from './features/categories/categories-list-private/categories-list-private.component';
import { CategoryDeleteComponent } from './features/categories/category-delete/category-delete.component';
import { CategoryEditComponent } from './features/categories/category-edit/category-edit.component';
import { CategoryAddComponent } from './features/categories/category-add/category-add.component';
import { LoginComponent } from './features/auth/login/login.component';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';
import { RegisterComponent } from './features/auth/register/register.component';
import { UsersListPrivateComponent } from './features/users/users-list-private/users-list-private.component';
import { UserDeleteComponent } from './features/users/user-delete/user-delete.component';
import { UserProfileComponent } from './features/users/user-profile/user-profile.component';
import { UserEditComponent } from './features/users/user-edit/user-edit.component';
import { LandingPageComponent } from './features/landing-page/landing-page/landing-page.component';
import { NotFoundComponent } from './shared/components/errors/not-found/not-found.component';
import { ValidationMessagesComponent } from './features/auth/register/register.component';
import { NotificationComponent } from './shared/components/modals/notification/notification.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ConfirmEmailComponent } from './features/auth/confirm-email/confirm-email.component';
import { SendEmailComponent } from './features/auth/send-email/send-email.component';
import { ResetPasswordComponent } from './features/auth/reset-password/reset-password.component';
import { TeamAddComponent } from './features/teams/team-add/team-add.component';
import { ViewTeamsInActivityComponent } from './features/teams/view-teams-in-activity/view-teams-in-activity.component';
import { TeamGetComponent } from './features/teams/team-get/team-get.component';
import { TeamEditComponent } from './features/teams/team-edit/team-edit.component';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { TeamMembershipRequestsAddComponent } from './features/team-membership-requests/team-membership-requests-add/team-membership-requests-add.component';
import { TeamMembershipRequestsViewComponent } from './features/team-membership-requests/team-membership-requests-view/team-membership-requests-view.component';
import { TeamMembershipRequestAcceptComponent } from './features/team-membership-requests/team-membership-request-accept/team-membership-request-accept.component';
import { TeamGetAllComponent } from './features/teams/team-get-all/team-get-all.component';
import { ActivityTeamReviewComponent } from './features/activities/activity-team-review/activity-team-review.component';
import { ApplyOrganizerComponent } from './features/organizer-applications/apply-organizer/apply-organizer.component';
import { ApplicationStatusComponent } from './features/organizer-applications/application-status/application-status.component';
import { AdminApplicationReviewComponent } from './features/organizer-applications/admin-application-review/admin-application-review.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { MatTabsModule } from '@angular/material/tabs';
import { GetUserteamsComponent } from './features/teams/get-userteams/get-userteams.component';
import { GetUserRequestsComponent } from './features/team-membership-requests/get-user-requests/get-user-requests.component';
import { PickWinnerComponent } from './features/activities/pick-winner/pick-winner.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { MatTableModule } from '@angular/material/table';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FooterComponent,
    ActivityListComponent,
    ActivityAddComponent,
    ActivityListPrivateComponent,
    ActivityEditComponent,
    ActivityDeleteComponent,
    ActivityGetComponent,
    ActivityTeamReviewComponent,
    UpdateAddComponent,
    UpdateListPrivateComponent,
    UpdateDeleteComponent,
    UpdateGetComponent,
    UpdateEditComponent,
    CategoriesListPrivateComponent,
    CategoryDeleteComponent,
    CategoryEditComponent,
    CategoryAddComponent,
    LoginComponent,
    RegisterComponent,
    UsersListPrivateComponent,
    UserDeleteComponent,
    UserProfileComponent,
    UserEditComponent,
    LandingPageComponent,
    NotFoundComponent,
    ValidationMessagesComponent,
    NotificationComponent,
    ConfirmEmailComponent,
    SendEmailComponent,
    ResetPasswordComponent,
    TeamAddComponent,
    ViewTeamsInActivityComponent,
    TeamGetComponent,
    TeamEditComponent,
    TeamGetAllComponent,
    GetUserteamsComponent,
    GetUserRequestsComponent,
    TeamMembershipRequestsAddComponent,
    TeamMembershipRequestsViewComponent,
    TeamMembershipRequestAcceptComponent,
    PickWinnerComponent,
    ApplyOrganizerComponent,
    ApplicationStatusComponent,
    AdminApplicationReviewComponent
  ],
  imports: [
    BrowserModule,
    FontAwesomeModule,
    CKEditorModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    MatTabsModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    DragDropModule,
    MatTableModule,
    ModalModule.forRoot()
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
