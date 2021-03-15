import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Notification } from '../_models/notification';
import { Profile } from '../_models/profile';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();
  
  constructor(private http: HttpClient) { }

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
  }

  getProfile(userName: string){
    return this.http.get<Profile>(this.baseUrl+ 'users/' + userName);
  }

}
