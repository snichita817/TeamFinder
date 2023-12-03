import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActivityListComponent } from './features/activities/activity-list/activity-list.component';
import { ActivityAddComponent } from './features/activities/activity-add/activity-add.component';
import { ActivityListPrivateComponent } from './features/activities/activity-list-private/activity-list-private.component';
import { ActivityEditComponent } from './features/activities/activity-edit/activity-edit.component';
import { ActivityDeleteComponent } from './features/activities/activity-delete/activity-delete.component';

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
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
