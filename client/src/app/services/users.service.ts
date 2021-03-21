import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, ReplaySubject } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Friend } from '../_models/friend';
import { Notification } from '../_models/notification';
import { Profile } from '../_models/profile';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor() { }

  setCurentUser(user: User){
    this.currentUserSource.next(user);
  }

  logoutUser(){
    this.currentUserSource.next(null);
  }

  updateNotification(notification: Notification){
    let temp: User;
    this.currentUser$.subscribe((user: User) => {
      temp = user;
    }).unsubscribe();
    temp.notification = notification;
    this.currentUserSource.next(temp);
    localStorage.setItem('user', JSON.stringify(temp));
  }

  updateFriendsList(friendsList: Friend[]){
    let temp: User;
    this.currentUser$.subscribe((user: User) => {
      temp = user;
    }).unsubscribe();
    temp.friends = friendsList;
    this.currentUserSource.next(temp);
    localStorage.setItem('user',JSON.stringify(temp));
  }

  

}
