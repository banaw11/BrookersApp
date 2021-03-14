import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { UnreadMessage } from '../_models/unreadMessage';
import { Notification } from '../_models/notification';
import { User } from '../_models/user';
import { UsersService } from './users.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private unreadMessagesSource = new BehaviorSubject<UnreadMessage[]>([]);
  unreadMessages$ = this.unreadMessagesSource.asObservable();

  constructor(private usersService: UsersService) {
    this.usersService.currentUser$.subscribe((user: User) => {
      if(user != null){
        this.unreadMessagesSource.next(user.notification.unreadMessages);
      }
      else {
        this.unreadMessagesSource.next(null);
      }
    })
   }
}
