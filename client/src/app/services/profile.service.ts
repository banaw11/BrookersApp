import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Profile } from '../_models/profile';


@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  baseUrl = environment.apiUrl;
  private profileSource = new BehaviorSubject<Profile>(null);
  profile$ = this.profileSource.asObservable();
  
  constructor(private http: HttpClient) { }

  getProfile(userName: string){
    this.http.get<Profile>(this.baseUrl + 'user/' + userName).pipe(take(1))
      .subscribe((profile: Profile) => {
        this.profileSource.next(profile);
    })
 }

 rememberProfile(){
  localStorage.setItem('profile', JSON.stringify(this.profileSource.value));
 }

 recoveryProfile(){
   this.profileSource.next(JSON.parse(localStorage.getItem('profile')))
   localStorage.removeItem('profile');
 }

 addFriend(){
   let friendId: number
   this.profile$.subscribe(p => {
     friendId = p.userId;
   }).unsubscribe();
    this.http.post(this.baseUrl+'user/send-invite/'+friendId, {});
 }

 deleteFriend(){
  let friendId: number
  this.profile$.subscribe(p => {
    friendId = p.userId;
  }).unsubscribe();
  this.http.post(this.baseUrl+'user/delete-friend/'+friendId, {});
 }

}