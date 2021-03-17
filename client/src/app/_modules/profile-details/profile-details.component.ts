import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { ChatService } from 'src/app/services/chat.service';
import { ProfileService } from 'src/app/services/profile.service';
import { Friend } from 'src/app/_models/friend';


@Component({
  selector: 'app-profile-details',
  templateUrl: './profile-details.component.html',
  styleUrls: ['./profile-details.component.scss']
})
export class ProfileDetailsComponent implements OnInit {
  @HostListener('window:beforeunload') refresh (){
    this.profileService.rememberProfile();
  }
  constructor(public profileService: ProfileService, private chatService: ChatService) { 
    
  }
  ngOnInit(): void {
    this.checkLocalStorage();
  }


  openChat(){
    let friend: Friend;
    this.profileService.profile$.subscribe(profile => {
      friend = {
        friendId: profile.userId,
        friendName: profile.userName,
        avatar: profile.image}
    }).unsubscribe();
    this.chatService.chatSbState.next("expanded");
    this.chatService.setFriend(friend);
  }

  checkLocalStorage(){
    let profile = localStorage.getItem('profile');
    if(profile){
      this.profileService.recoveryProfile();
    }
  }
}
