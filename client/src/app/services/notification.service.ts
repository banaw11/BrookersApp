import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { Message } from '../_models/message';
import { UnreadMessage } from '../_models/unreadMessage';
import { User } from '../_models/user';
import { UsersService } from './users.service';

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

   getUnreadMessages(senderId: number){
     let tempList : UnreadMessage[];
     this.unreadMessages$.subscribe(unread => {
       tempList = unread.filter(x => x.senderId == senderId);
     }).unsubscribe();
     return tempList;
   }

   markAsReadMessage(messageIDs: number[]){
     let tempList : UnreadMessage[] = [];
     this.unreadMessages$.subscribe(unreadMessages => {
       tempList = unreadMessages.filter(x => !messageIDs.includes(x.messageId))
     }).unsubscribe();
     this.unreadMessagesSource.next(tempList);
   }

   addUnreadMessage(message : Message){
     let unreadMessage: UnreadMessage = {
       messageId: message.id,
       senderId: message.senderId
     }
     this.unreadMessagesSource.next([...this.unreadMessagesSource.value, unreadMessage]);
   }

   isUnreadMessage(friendId: number) : boolean {
     return this.unreadMessagesSource.value.some(x => x.senderId == friendId);
   }
}
