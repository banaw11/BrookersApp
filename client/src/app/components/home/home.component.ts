import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NbMenuItem, NbMenuService, NbSidebarService } from '@nebular/theme';
import { filter, map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/services/account.service';
import { UsersService } from 'src/app/services/users.service';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit  {
user: User;
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
  constructor(private sidebarService: NbSidebarService, public usersService: UsersService, private menuService: NbMenuService, private accountservice: AccountService, private router: Router) {
    this.usersService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
    })
   }
  ngOnInit(): void {
    this.onMenuClick();
  }


  toggle(){
    this.sidebarService.toggle(this.compacted, 'menu-sidebar');
    this.compacted = !this.compacted;
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

}
