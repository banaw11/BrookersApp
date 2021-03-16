import { Component, OnInit, ViewChild } from '@angular/core';
import { HomeComponent } from 'src/app/components/home/home.component';
import { ProfileService } from 'src/app/services/profile.service';
import { UsersService } from 'src/app/services/users.service';
import { Friend } from 'src/app/_models/friend';
import { Profile } from 'src/app/_models/profile';
import { ChatSidebarComponent } from '../chat-sidebar/chat-sidebar.component';


@Component({
  selector: 'app-profile-details',
  templateUrl: './profile-details.component.html',
  styleUrls: ['./profile-details.component.scss']
})
export class ProfileDetailsComponent implements OnInit {
  @ViewChild(HomeComponent) Home : HomeComponent;
  @ViewChild(ChatSidebarComponent) Chat : ChatSidebarComponent;
  constructor(public profileService: ProfileService) { 
    
  }

  ngOnInit(): void {
    
  }

  openChat(){
    console.log("cos")
    let friend: Friend;
    this.profileService.profile$.subscribe(profile => {
      friend = {
        friendId: profile.userId,
        friendName: profile.userName,
        avatar: profile.image}
    }).unsubscribe();
    this.Home.chatSbState.next("expanded");
    this.Chat.setFriend(friend);
  }

}
