import { Component } from '@angular/core';
import { AddUpdateRequest } from '../models/update-add-request.model';
import { Subscription } from 'rxjs';
import { UpdateService } from '../services/update.service';
import { ActivatedRoute, Router } from '@angular/router';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { ChangeEvent } from '@ckeditor/ckeditor5-angular';
import { SharedService } from 'src/app/shared/shared.service';

@Component({
  selector: 'app-update-add',
  templateUrl: './update-add.component.html',
  styleUrls: ['./update-add.component.css']
})
export class UpdateAddComponent {
  public Editor = ClassicEditor;
  model: AddUpdateRequest;
  activityId: string | null = null;

  private routeSubscription?: Subscription;
  private addUpdateSubscription?: Subscription;

  constructor(private updateService: UpdateService,
    private route: ActivatedRoute,
    private router: Router,
    private sharedService: SharedService){
      this.model = {
        title: '',
        text: '',
        date: new Date(),
        activityId: '',
      }
  }

  public onReady(editor: any) {
    console.log("CKEditor5 Angular Component is ready to use!", editor);
  }
  public onChange({ editor }: ChangeEvent) {
    const data = editor.getData();
  }

  onFormSubmit()
  {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.activityId = params.get('activityId')

        if(this.activityId) {
          this.model.activityId = this.activityId;
          this.addUpdateSubscription = this.updateService.addUpdate(this.model)
          .subscribe({
            next: (response) => {
              this.router.navigateByUrl(`activities/get/${this.activityId}`,);
            },
            error: (error) => {
              console.log(error)
              if(error.error) {
                this.sharedService.showNotification(false, 'Error!', error.error);
              }
            }
          })
        }
      }
    })
  }

  ngOnDestroy() {
    this.routeSubscription?.unsubscribe();
    this.addUpdateSubscription?.unsubscribe();
  }
}
