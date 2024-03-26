import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './core/components/navbar/navbar.component';
import { ActivityListComponent } from './features/activities/activity-list/activity-list.component';
import { ActivityAddComponent } from './features/activities/activity-add/activity-add.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
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

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    ActivityListComponent,
    ActivityAddComponent,
    ActivityListPrivateComponent,
    ActivityEditComponent,
    ActivityDeleteComponent,
    ActivityGetComponent,
    UpdateAddComponent,
    UpdateListPrivateComponent,
    UpdateDeleteComponent,
    UpdateGetComponent,
    UpdateEditComponent,
    CategoriesListPrivateComponent,
    CategoryDeleteComponent,
    CategoryEditComponent,
    CategoryAddComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
