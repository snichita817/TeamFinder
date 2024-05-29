import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActivityListComponent } from './features/activities/activity-list/activity-list.component';
import { ActivityAddComponent } from './features/activities/activity-add/activity-add.component';
import { ActivityListPrivateComponent } from './features/activities/activity-list-private/activity-list-private.component';
import { ActivityEditComponent } from './features/activities/activity-edit/activity-edit.component';
import { ActivityDeleteComponent } from './features/activities/activity-delete/activity-delete.component';
import { ActivityGetComponent } from './features/activities/activity-get/activity-get.component';
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
import { RegisterComponent } from './features/auth/register/register.component';
import { UsersListPrivateComponent } from './features/users/users-list-private/users-list-private.component';
import { UserDeleteComponent } from './features/users/user-delete/user-delete.component';
import { UserProfileComponent } from './features/users/user-profile/user-profile.component';
import { UserEditComponent } from './features/users/user-edit/user-edit.component';
import { LandingPageComponent } from './features/landing-page/landing-page/landing-page.component';
import { NotFoundComponent } from './shared/components/errors/not-found/not-found.component';
import { ConfirmEmailComponent } from './features/auth/confirm-email/confirm-email.component';
import { SendEmailComponent } from './features/auth/send-email/send-email.component';
import { ResetPasswordComponent } from './features/auth/reset-password/reset-password.component';
import { TeamAddComponent } from './features/teams/team-add/team-add.component';
import { ViewTeamsInActivityComponent } from './features/teams/view-teams-in-activity/view-teams-in-activity.component';
import { TeamGetComponent } from './features/teams/team-get/team-get.component';
import { TeamEditComponent } from './features/teams/team-edit/team-edit.component';
import { TeamMembershipRequestsAddComponent } from './features/team-membership-requests/team-membership-requests-add/team-membership-requests-add.component';
import { TeamMembershipRequestsViewComponent } from './features/team-membership-requests/team-membership-requests-view/team-membership-requests-view.component';
import { TeamMembershipRequestAcceptComponent } from './features/team-membership-requests/team-membership-request-accept/team-membership-request-accept.component';
import { TeamGetAllComponent } from './features/teams/team-get-all/team-get-all.component';
import { ActivityTeamReviewComponent } from './features/activities/activity-team-review/activity-team-review.component';
import { ApplyOrganizerComponent } from './features/organizer-applications/apply-organizer/apply-organizer.component';
import { ApplicationStatusComponent } from './features/organizer-applications/application-status/application-status.component';
import { AdminApplicationReviewComponent } from './features/organizer-applications/admin-application-review/admin-application-review.component';
import { adminAuthGuard } from './features/auth/guards/auth.guard';
import { organizerAuthGuard } from './features/auth/guards/auth.guard';

const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'activities', component: ActivityListComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'admin/users', component: UsersListPrivateComponent, canActivate: [adminAuthGuard] },
  { path: 'users/delete/:id', component: UserDeleteComponent, canActivate: [adminAuthGuard] },
  { path: 'user/:id', component: UserProfileComponent },
  { path: 'user/edit/:id', component: UserEditComponent },
  { path: 'account/confirm-email', component: ConfirmEmailComponent },
  { path: 'account/send-email/:mode', component: SendEmailComponent },
  { path: 'account/reset-password', component: ResetPasswordComponent },
  { path: 'activities/add', component: ActivityAddComponent, canActivate: [organizerAuthGuard] },
  { path: 'admin/activities', component: ActivityListPrivateComponent, canActivate: [adminAuthGuard] },
  { path: 'admin/applications-review', component: AdminApplicationReviewComponent, canActivate: [adminAuthGuard] },
  { path: 'activities/edit/:id', component: ActivityEditComponent, canActivate: [organizerAuthGuard] },
  { path: 'activities/delete/:id', component: ActivityDeleteComponent, canActivate: [adminAuthGuard] },
  { path: 'activity/:id/teams/review', component: ActivityTeamReviewComponent, canActivate: [organizerAuthGuard] },
  { path: 'activities/get/:id', component: ActivityGetComponent },
  { path: 'updates/add/:activityId', component: UpdateAddComponent, canActivate: [organizerAuthGuard] },
  { path: 'updates/delete/:id', component: UpdateDeleteComponent },
  { path: 'updates/get/:id', component: UpdateGetComponent },
  { path: 'updates/edit/:id', component: UpdateEditComponent, canActivate: [organizerAuthGuard] },
  { path: 'admin/updates', component: UpdateListPrivateComponent, canActivate: [adminAuthGuard] },
  { path: 'admin/categories', component: CategoriesListPrivateComponent, canActivate: [adminAuthGuard] },
  { path: 'categories/delete/:id', component: CategoryDeleteComponent, canActivate: [adminAuthGuard] },
  { path: 'categories/edit/:id', component: CategoryEditComponent, canActivate: [adminAuthGuard] },
  { path: 'categories/add', component: CategoryAddComponent, canActivate: [adminAuthGuard] },
  { path: 'team/view/:id', component: TeamGetComponent },
  { path: 'team/edit/:id', component: TeamEditComponent, canActivate: [organizerAuthGuard] },
  { path: 'teams', component: TeamGetAllComponent },
  { path: 'activity/:activityId/teams/register', component: TeamAddComponent, canActivate: [organizerAuthGuard] },
  { path: 'activity/teams/:activityId', component: ViewTeamsInActivityComponent },
  { path: 'team/membership-request/:teamId', component: TeamMembershipRequestsAddComponent },
  { path: 'team/membership-requests/:teamId', component: TeamMembershipRequestsViewComponent },
  { path: 'team/membership-request/:memId/:res', component: TeamMembershipRequestAcceptComponent, canActivate: [organizerAuthGuard] },
  { path: 'apply-organizer', component: ApplyOrganizerComponent },
  { path: 'application-status', component: ApplicationStatusComponent },
  { path: '**', component: NotFoundComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
