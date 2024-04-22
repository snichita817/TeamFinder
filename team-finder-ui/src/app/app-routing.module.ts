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
import { authGuard } from './features/auth/guards/auth.guard';
import { RegisterComponent } from './features/auth/register/register.component';
import { UsersListPrivateComponent } from './features/users/users-list-private/users-list-private.component';
import { UserDeleteComponent } from './features/users/user-delete/user-delete.component';
import { UserProfileComponent } from './features/users/user-profile/user-profile.component';
import { UserEditComponent } from './features/users/user-edit/user-edit.component';
import { LandingPageComponent } from './features/landing-page/landing-page/landing-page.component';

const routes: Routes = [
  {
    path: '',
    component: LandingPageComponent
  },
  {
    path: 'activities',
    component: ActivityListComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'admin/users',
    component: UsersListPrivateComponent
  },
  {
    path: 'users/delete/:id',
    component: UserDeleteComponent
  },
  {
    path: 'user/:id',
    component: UserProfileComponent
  },
  {
    path: 'user/edit/:id',
    component: UserEditComponent
  },
  {
    path: 'activities/add',
    component: ActivityAddComponent
  },
  {
    path: 'admin/activities',
    component: ActivityListPrivateComponent,
    canActivate: [authGuard]
  },
  {
    path: 'activities/edit/:id',
    component: ActivityEditComponent,
  },
  {
    path: 'activities/delete/:id',
    component: ActivityDeleteComponent,
    canActivate: [authGuard]
  },
  {
    path: 'activities/get/:id',
    component: ActivityGetComponent
  },
  {
    path: 'updates/add/:activityId',
    component: UpdateAddComponent
  },
  {
    path: 'updates/delete/:id',
    component: UpdateDeleteComponent,
    canActivate: [authGuard]
  },
  {
    path: 'updates/get/:id',
    component: UpdateGetComponent
  },
  {
    path: 'updates/edit/:id',
    component: UpdateEditComponent
  },
  {
    path: 'admin/updates',
    component: UpdateListPrivateComponent,
    canActivate: [authGuard]
  },
  {
    path: 'admin/categories',
    component: CategoriesListPrivateComponent,
    canActivate: [authGuard]
  },
  {
    path: 'categories/delete/:id',
    component: CategoryDeleteComponent
  },
  {
    path: 'categories/edit/:id',
    component: CategoryEditComponent
  },
  {
    path: 'categories/add',
    component: CategoryAddComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
