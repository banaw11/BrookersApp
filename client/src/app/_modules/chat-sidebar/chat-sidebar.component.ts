import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, Observable, pipe } from 'rxjs';
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
  private messagesSource = new BehaviorSubject<Message[]>([]);
  messages$ = this.messagesSource.asObservable();
  private chatMemberSource = new BehaviorSubject<Friend>(null);
  chatMember$ = this.chatMemberSource.asObservable();
  messageThread : MessageThread = {
    chatMember: this.chatMember$,
    messages: this.messages$
  }
  private friendList = new BehaviorSubject<Friend[]>([]);
  friendList$ = this.friendList.asObservable();

  constructor(private usersService: UsersService, private chatService: ChatService) { 
    this.usersService.currentUser$.pipe(take(1)).subscribe( (user: User) =>{
      this.user = user;
      this.friendList.next(user.friends);
    }).unsubscribe();
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
  this.chatMemberSource.next(friend);
  this.chatService.getMessageThread(friend.friendId).pipe().subscribe(result => {
    this.messagesSource.next(result);
  })
}

sendMessage(event: any){
  let friendId :number;
  this.chatMember$.subscribe(f => friendId = f.friendId).unsubscribe();
  let message: NewMessage = this.createMessage(event.message, friendId); 
  this.chatService.sendMessage(message).pipe().subscribe(msg => {
      if(msg){
        this.addMessage(msg);
      }
    })
}


createMessage(content: string, id: number){
 let message : NewMessage = {
    receiverId: id,
    content: content};
  return message;
}

addMessage(msg: Message){
  this.messages$.subscribe(msgs => {
    msgs.push(msg);
  }).unsubscribe();
  this.messages$.pipe(map(msgs => this.messagesSource.next(msgs)));
}

}
