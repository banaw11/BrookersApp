import { LocationStrategy, PlatformLocation } from '@angular/common';
import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NbMenuItem, NbMenuService, NbSidebarService, NbSidebarState } from '@nebular/theme';
import { BehaviorSubject, Observable, observable } from 'rxjs';
import { filter, flatMap, map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/services/account.service';
import { NotificationService } from 'src/app/services/notification.service';
import { PressenceService } from 'src/app/services/pressence.service';
import { UsersService } from 'src/app/services/users.service';
import { Friend } from 'src/app/_models/friend';
import { User } from 'src/app/_models/user';
import { ChatSidebarComponent } from 'src/app/_modules/chat-sidebar/chat-sidebar.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit  {
  @ViewChild(ChatSidebarComponent) sidebarChat: ChatSidebarComponent;
   menuSbState = new BehaviorSubject<NbSidebarState>('compacted');
   chatSbState = new BehaviorSubject<NbSidebarState>('collapsed');
user: User;
searchString = new BehaviorSubject<string>(null);
userMenu = [{title: 'Logout'}];
compacted: boolean= true;
menuItems: NbMenuItem[] = [
  {
    title: "Home",
    icon: "home",
    link: '/home',
    home: true,
  },
  {
    title:"Community",
    icon: "globe",
    link: "/home/community"
  },
  {
    title: "Investments",
    icon:"trending-up",
    link: "/home/investments"
  },
  {
    title:"Settings",
    icon: "settings",
    link: "/home/settings"
  }
]

  constructor( public usersService: UsersService, private menuService: NbMenuService, private accountservice: AccountService, private router: Router
    , private pressenceService: PressenceService, private location: LocationStrategy, public notificationService: NotificationService) {
    this.usersService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
    });
    history.pushState(null,null, window.location.href);
    this.location.onPopState(() => {
      history.pushState(null,null, window.location.href);
    })
   }
  ngOnInit(): void {
    this.onMenuClick();
    this.getUserFromLocalStorage();
  }

  toggleChat(tag: string){
    tag === 'menu-sidebar' ?
    this.menuSbState.value === 'compacted' ? this.menuSbState.next('collapsed') : this.menuSbState.next('compacted'):
    this.chatSbState.value === 'collapsed' ? this.chatSbState.next('expanded') : this.chatSbState.next('collapsed');
  }

  onMenuClick(){
    this.menuService.onItemClick().pipe(
      filter(({tag}) => tag === 'user-menu'),
      map(({item: {title}}) => title),
    ).subscribe(title => {
      if(title === 'Logout'){
        this.accountservice.logout();
        this.router.navigateByUrl('');
      }
    })
  }

  onKeyUp(event : any){
    this.sidebarChat.search(event.target.value);
  }

  getUserFromLocalStorage(){
    let user:User = JSON.parse(localStorage.getItem('user'));
    this.pressenceService.tryReconnectHubConnection(user);
  }
}
