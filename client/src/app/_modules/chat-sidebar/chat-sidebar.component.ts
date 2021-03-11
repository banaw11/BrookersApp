import { Component, OnInit } from '@angular/core';
import { ChatService } from 'src/app/services/chat.service';
import { PressenceService } from 'src/app/services/pressence.service';
import { Friend } from 'src/app/_models/friend';

@Component({
  selector: 'app-chat-sidebar',
  templateUrl: './chat-sidebar.component.html',
  styleUrls: ['./chat-sidebar.component.scss']
})
export class ChatSidebarComponent implements OnInit{
  dataLoadded : Promise<boolean>
  constructor(public chatService: ChatService, private pressenceService: PressenceService) { 
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
}

sendMessage(event: any){
  this.chatService.createMessage(event.message);
}
}
