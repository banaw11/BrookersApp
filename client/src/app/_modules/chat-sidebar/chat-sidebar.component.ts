import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { UsersService } from 'src/app/services/users.service';
import { Friend } from 'src/app/_models/friend';
import { Message } from 'src/app/_models/message';
import { MessageThread } from 'src/app/_models/messageThread';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-chat-sidebar',
  templateUrl: './chat-sidebar.component.html',
  styleUrls: ['./chat-sidebar.component.scss']
})
export class ChatSidebarComponent implements OnInit {
  user: User;
  messageThread = new BehaviorSubject<MessageThread>({chatMember: null,messages:[]});
  private friendList = new BehaviorSubject<Friend[]>([]);
  friendList$ = this.friendList.asObservable();
  constructor(private usersService: UsersService) { 
    this.usersService.currentUser$.pipe(take(1)).subscribe( (user: User) =>{
      this.user = user;
      this.friendList.next(user.friends);
    })
  }

  ngOnInit(): void {
  }

  search(str: string){
  this.friendList.next(this.getResults(str))
}

getResults(m : string){
  return this.user.friends.filter(f => { return f.friendName.toLowerCase().startsWith(m.toLowerCase())})
}

setFriend(friend : Friend){
  this.usersService.getMessageThread(friend.friendId).pipe().subscribe( (result: Message[]) =>{
    this.messageThread.next({chatMember: friend, messages: result});
  })
}

}
