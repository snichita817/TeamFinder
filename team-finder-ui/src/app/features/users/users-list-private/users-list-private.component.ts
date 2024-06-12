import { Component } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { User } from '../models/user.model';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-users-list-private',
  templateUrl: './users-list-private.component.html',
  styleUrl: './users-list-private.component.css'
})
export class UsersListPrivateComponent {
  users$?: Observable<User[]>;

  userServiceSubscription?: Subscription;
  allRoles = ["Admin", "Organizer", "User"]

  constructor(private userService: UserService,
    private router: Router) { }

  ngOnInit()
  {
    this.users$ = this.userService.indexUsers();
  }

  assignOrganizer(email: string) {
    this.userServiceSubscription = this.userService.addRole(email).subscribe({
      next: (response) => {
        this.router.navigateByUrl('/');
      }
    })
  }

  ngOnDestroy() {
    this.userServiceSubscription?.unsubscribe();
  }
}
