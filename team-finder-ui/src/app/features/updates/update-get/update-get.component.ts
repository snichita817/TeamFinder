import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { UpdateService } from '../services/update.service';
import { Update } from '../models/update.model';
import { Comment } from '../../comments/models/comment.model';
import { AddCommentDto } from '../../comments/models/add-comment.model';
import { CommentService } from '../../comments/services/comment.service';
import { AuthService } from '../../auth/services/auth.service';
import { SharedService } from 'src/app/shared/shared.service';

@Component({
  selector: 'app-update-get',
  templateUrl: './update-get.component.html',
  styleUrls: ['./update-get.component.css']
})
export class UpdateGetComponent implements OnInit, OnDestroy {
  updateId: string | null = null;
  model?: Update;
  comments: Comment[] = [];
  newCommentText: string = '';
  userId: string | undefined; // Retrieve actual user data
  userName: string = ''; // Retrieve actual user data

  routeSubscription?: Subscription;
  getUpdateSubscription?: Subscription;
  getCommentsSubscription?: Subscription;
  postCommentSubscription?: Subscription;
  deleteCommentSubscription?: Subscription;

  constructor(private updateService: UpdateService, 
    private route: ActivatedRoute,
    private commentService: CommentService,
    private authService: AuthService,
    private sharedService: SharedService,
    private router: Router) {}

  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe(params => {
      this.updateId = params.get('id');

      if (this.updateId) {
        this.getUpdateSubscription = this.updateService.getUpdate(this.updateId).subscribe(result => {
          this.model = result;
        });

        this.getCommentsSubscription = this.commentService.getComments(this.updateId).subscribe(result => {
          this.comments = result;
        });
      }
    });

    this.userId = this.authService.getUser()?.id; // Replace with actual user retrieval logic
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.getUpdateSubscription?.unsubscribe();
    this.getCommentsSubscription?.unsubscribe();
    this.postCommentSubscription?.unsubscribe();
    this.deleteCommentSubscription?.unsubscribe();
  }

  postComment(): void {
    if(!this.userId) {
      this.sharedService.showNotification(false, "Error!", "You should first login before posting a comment!");
      this.router.navigateByUrl('/login');
    }
    if (this.updateId && this.newCommentText && this.userId) {
      const newComment: AddCommentDto = {
        text: this.newCommentText,
        userId: this.userId,
        updateId: this.updateId
      };

      this.postCommentSubscription = this.commentService.postComment(newComment).subscribe(result => {
        this.comments.push(result);
        this.newCommentText = '';
      });
    }
  }

  deleteComment(id: string) {
    this.deleteCommentSubscription = this.commentService.deleteComment(id).subscribe({
      next: (response) => {
        this.ngOnInit();
      }
    })
  }
}
