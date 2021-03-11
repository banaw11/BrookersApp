import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { ChatService } from './chat.service';

@Injectable({
  providedIn: 'root'
})
export class PressenceService {
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  private onlineFriendsSource = new BehaviorSubject<number[]>([]);
  onlineFriends$ = this.onlineFriendsSource.asObservable();

  constructor(private chatService: ChatService) { }

  createHubConnection(user: User){
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'pressence', {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build()

      this.hubConnection.start().catch(error => console.log(error));

      this.hubConnection.on('FriendIsOnline', userId => {
        this.onlineFriends$.pipe(take(1)).subscribe(userIds => {
          this.onlineFriendsSource.next([...userIds, userId]);
          this.chatService.getStatus(userId, 'success');
        })
      })

      this.hubConnection.on('FriendIsOfline', userId => {
        this.onlineFriends$.pipe(take(1)).subscribe(userIds => {
          this.onlineFriendsSource.next([...userIds.filter(x => x !== userId)]);
          this.chatService.getStatus(userId,'basic');
        })
      })

      this.hubConnection.on("GetOnlineFriends", userIds => {
        this.onlineFriendsSource.next(userIds);
        this.chatService.pressenceList = userIds;
      })
  }

  stopHubConnection(){
    if(this.hubConnection){
      this.hubConnection.stop().catch(error => console.log(error));
    }
    
  }

  tryReconnectHubConnection(user: User){
    if(!this.hubConnection){
      this.createHubConnection(user);
    }
  }

}
