import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-users-list-private',
  templateUrl: './users-list-private.component.html',
  styleUrl: './users-list-private.component.css'
})
export class UsersListPrivateComponent {
  users$?: Observable<User[]>;

  constructor(private userService: UserService) { }

  ngOnInit()
  {
    this.users$ = this.userService.indexUsers();
  }
}
