import { Component, OnInit } from '@angular/core';
import { UsersService } from './services/users.service';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Brookers Community App';
  constructor(private usersService: UsersService){
  
  }
  ngOnInit(): void {
    this.checkCurrentUser();
  }

  checkCurrentUser(){
    const user: User = JSON.parse(localStorage.getItem('user')); 
    this.usersService.setCurentUser(user);
  }
}
