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
    UpdateAddComponent
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
