import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { ChatService } from 'src/app/services/chat.service';
import { UsersService } from 'src/app/services/users.service';
import { Friend } from 'src/app/_models/friend';
import { Message } from 'src/app/_models/message';
import { MessageThread } from 'src/app/_models/messageThread';
import { NewMessage } from 'src/app/_models/newMessage';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-chat-sidebar',
  templateUrl: './chat-sidebar.component.html',
  styleUrls: ['./chat-sidebar.component.scss']
})
export class ChatSidebarComponent implements OnInit {
  user: User;
  messageThread = new BehaviorSubject<MessageThread>({chatMember: null,messages: null});
  private friendList = new BehaviorSubject<Friend[]>([]);
  friendList$ = this.friendList.asObservable();

  constructor(private usersService: UsersService, private chatService: ChatService) { 
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
  this.messageThread.next(
    {
      chatMember: friend, 
      messages: this.chatService.getMessageThread(friend.friendId)});
}

sendMessage(event: any){
  this.chatService.sendMessage(this.createMessage(event.message)).pipe().subscribe( response => {
      // if(response){
      //   this.messageThread.subscribe(msgT => {
      //     msgT.messages.pipe(take(1)).subscribe(msgs => {
      //       msgs.push(response);
      //       this.messageThread.next(msgT);
      //     })
      //   });
      //   this.messageThread.value.messages.pipe(
      //     map((msgs : Message[]) =>{
      //       msgs.push(response);
      //       this.messageThread.next({chatMember: this.messageThread.value.chatMember,messages : });
      //     })
      //   )
      // }
  }, error => {
    console.log(error.error);
  }
  )
}

createMessage(content: string){
 let message : NewMessage = {
    receiverId: this.messageThread.value.chatMember.friendId,
    content: content};
  return message;
}

}
