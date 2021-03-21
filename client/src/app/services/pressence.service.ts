import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Friend } from '../_models/friend';
import { Message } from '../_models/message';
import { Notification } from '../_models/notification';
import { User } from '../_models/user';
import { ChatService } from './chat.service';
import { NotificationService } from './notification.service';
import { UsersService } from './users.service';

@Injectable({
  providedIn: 'root'
})
export class PressenceService {
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  private onlineFriendsSource = new BehaviorSubject<number[]>([]);
  onlineFriends$ = this.onlineFriendsSource.asObservable();

  constructor(private chatService: ChatService, private notificationService: NotificationService,  private usersService: UsersService ) { }

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

      this.hubConnection.on("GetNewMessage", (message : Message) => {
        this.chatService.getMessage(message) ? 
        this.markAsRead(message.id) :
          this.notificationService.addUnreadMessage(message) 
      })

      this.hubConnection.on("AddMessage", (message: Message) => {
        this.chatService.getMessage(message);
      })

      this.hubConnection.on("UnreadMessagesRefreshed", (notification: Notification) => {
        this.usersService.updateNotification(notification)
      })

      this.hubConnection.on("GetInvite", (invite: Friend) => {
        let tempNotification: Notification;
        this.usersService.currentUser$.subscribe((user: User) => {
          tempNotification = user.notification;
        }).unsubscribe();
        tempNotification.invitations.push(invite);
        this.usersService.updateNotification(tempNotification);
      })

      this.hubConnection.on("InviteAccepted", (invite: Friend) => {
        console.log("invite");
      })

      this.hubConnection.on("RefreshedFriendsList", (friendList: Friend[]) => {
        this.usersService.updateFriendsList(friendList);
      })

      this.hubConnection.on("InvitationsRefreshed", (invitationsList: Friend[]) => {
        let tempNotification: Notification;
        this.usersService.currentUser$.subscribe((user: User) => {
          tempNotification = user.notification;
        }).unsubscribe();
        tempNotification.invitations = invitationsList;
        this.usersService.updateNotification(tempNotification);
      })
  }

  async markAsRead(messageId: number){
    return this.hubConnection.invoke("MarkAsReadMessage", messageId).catch(error => console.error(error));
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
