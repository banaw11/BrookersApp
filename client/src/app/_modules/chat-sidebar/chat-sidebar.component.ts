import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ChatService } from 'src/app/services/chat.service';
import { NotificationService } from 'src/app/services/notification.service';
import { PressenceService } from 'src/app/services/pressence.service';
import { Friend } from 'src/app/_models/friend';
import { UnreadMessage } from 'src/app/_models/unreadMessage';

@Component({
  selector: 'app-chat-sidebar',
  templateUrl: './chat-sidebar.component.html',
  styleUrls: ['./chat-sidebar.component.scss']
})
export class ChatSidebarComponent implements OnInit{
  dataLoadded : Promise<boolean>
  constructor(public chatService: ChatService, private pressenceService: PressenceService, private notificationService: NotificationService, private router: Router) { 
    this.pressenceService.onlineFriends$.subscribe(pList => {
      this.chatService.pressenceList = pList;
    }).unsubscribe()
  }
  ngOnInit(): void {
    this.chatService.getFriends();
  }

  search(name: string){
  this.chatService.searchFriend(name);
}

setFriend(friend : Friend){
  this.chatService.setFriend(friend);
  this.markAsRead();
}

sendMessage(event: any){
  this.chatService.createMessage(event.message);
}

markAsRead(){
  let unreadMessages : UnreadMessage[];
  this.chatService.chatMember$.subscribe(friend => {
    unreadMessages = this.notificationService.getUnreadMessages(friend.friendId)
  }).unsubscribe();
  if(unreadMessages.length > 0){
    unreadMessages.forEach(element => {
      this.pressenceService.markAsRead(element.messageId)
    });
  }
}

goToProfile(friendName: string){
  this.router.navigate(['/home/profile', friendName]);
}


}
