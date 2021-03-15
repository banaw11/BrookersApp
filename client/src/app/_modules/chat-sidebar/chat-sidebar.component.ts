import { Component, OnInit } from '@angular/core';
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
  constructor(public chatService: ChatService, private pressenceService: PressenceService, private notificationService: NotificationService) { 
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
  let messageIDs : number[] = [];
  this.chatService.chatMember$.subscribe(friend => {
    unreadMessages = this.notificationService.getUnreadMessages(friend.friendId)
  }).unsubscribe();
  if(unreadMessages.length > 0){
    unreadMessages.forEach(element => {
      this.pressenceService.markAsRead(element.messageId) ?
        messageIDs.push(element.messageId) : null
    });
    messageIDs.length > 0 ? this.notificationService.markAsReadMessage(messageIDs) : null
  }
}


}
