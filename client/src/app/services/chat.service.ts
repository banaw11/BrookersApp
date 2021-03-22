import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NbSidebarState } from '@nebular/theme';
import { BehaviorSubject } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Friend } from '../_models/friend';
import { Message } from '../_models/message';
import { MessageThread } from '../_models/messageThread';
import { NewMessage } from '../_models/newMessage';
import { UnreadMessage } from '../_models/unreadMessage';
import { UsersService } from './users.service';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
baseUrl = environment.apiUrl;
  private friendList = new BehaviorSubject<Friend[]>([]);
  friendList$ = this.friendList.asObservable();
private chatMemberSource = new BehaviorSubject<Friend>(null);
chatMember$ = this.chatMemberSource.asObservable();
  private messagesSource = new BehaviorSubject<Message[]>([]);
  messages$ = this.messagesSource.asObservable();
messageThread: MessageThread = {
  chatMember: this.chatMember$,
  messages: this.messages$
}
  pressenceList: number[];
  chatSbState = new BehaviorSubject<NbSidebarState>('collapsed');
  constructor(private http: HttpClient, private usersService: UsersService) {
    
   }

  getFriends(){
    this.usersService.currentUser$.subscribe( user => {
      this.friendList.next(user.friends);
    }).unsubscribe();
    this.setStatuses();
  }

  searchFriend(name: string){
    name != '' ? this.getResults(name) : this.getFriends(); 
  }

  getResults(name: string){
    let tempList: Friend[];
    this.friendList$.subscribe(fList => {
      tempList = fList.filter(f => { return f.friendName.toLowerCase().startsWith(name.toLowerCase())});
    }).unsubscribe();
    this.friendList.next(tempList);
  }

  setFriend(friend: Friend){
    this.chatMemberSource.next(friend);
    this.getMessages(friend.friendId).subscribe(messages => {
      this.messagesSource.next(messages);
    })
  }

  getMessages(memberId : number){
    return this.http.get<Message[]>(this.baseUrl + "chat/"+ memberId);
  }

  createMessage(content: string){
    let friendId: number;
    this.chatMember$.subscribe(f => friendId = f.friendId).unsubscribe();
    let message : NewMessage = {
      receiverId: friendId,
      content: content};
    this.sendMessage(message);
  }

  sendMessage(message: NewMessage){
    this.http.post(this.baseUrl + "chat/new-message", message).subscribe();
  }

  addMessage(msg: Message){
      this.messages$.subscribe(msgs => {
        msgs.push(msg);
      }).unsubscribe();
      this.messages$.pipe(map(msgs => this.messagesSource.next(msgs)));
  }

  getMessage(msg: Message){
    let currentFriendId: number;
    this.chatMember$.subscribe(f => {
      if(f != null){
        currentFriendId = f.friendId;
      }
    }).unsubscribe()
    if(currentFriendId === msg.senderId || currentFriendId === msg.receiverId){
      this.addMessage(msg);
      return true;
    }
    return false;
  }

  setStatuses(){
    let tempList: Friend[];
      this.friendList$.subscribe(fList => {
        fList.forEach( f => {
          this.pressenceList.includes(f.friendId) ? f.status='success' : f.status='basic';
        })
        tempList = fList;
      }).unsubscribe();
    this.friendList.next(tempList);
  }
  
  getStatus(friendId: number, status: string){
      this.friendList$.subscribe(fList => {
        fList.find(x=> x.friendId == friendId).status = status;
      })
  }

  cleanMessageThread(){
    this.chatMemberSource.value ? this.chatMemberSource.next(null) : null ;
    this.messagesSource.value ? this.messagesSource.next(null) : null ;
  }

  toggleChat(){
    this.chatSbState.value === 'collapsed' ? this.chatSbState.next('expanded') : this.chatSbState.next('collapsed');
    this.cleanMessageThread();
  }

}
