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

const routes: Routes = [
  {
    path: '',
    component: ActivityListComponent
  },
  {
    path: 'activities/add',
    component: ActivityAddComponent
  },
  {
    path: 'admin/activities',
    component: ActivityListPrivateComponent
  },
  {
    path: 'activities/edit/:id',
    component: ActivityEditComponent
  },
  {
    path: 'activities/delete/:id',
    component: ActivityDeleteComponent
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
    component: UpdateDeleteComponent
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
    component: UpdateListPrivateComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
