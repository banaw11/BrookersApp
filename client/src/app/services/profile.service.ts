import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { BehaviorSubject } from 'rxjs';
import { map, take } from 'rxjs/operators';
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
        console.log(profile)
        this.profileSource.next(profile);
    })
 }

 rememberProfile(){
  localStorage.setItem('profile', JSON.stringify(this.profileSource.value));
 }

 recoveryProfile(){
   let profile: Profile = JSON.parse(localStorage.getItem('profile'));
   this.getProfile(profile.userName);
   localStorage.removeItem('profile');
 }

 addFriend(){
   let friendId: number
   this.profile$.subscribe(p => {
     friendId = p.userId;
   }).unsubscribe();
    return this.http.post(this.baseUrl+'user/send-invite/'+friendId, {}).pipe(
      map((response: boolean) => {return response})
    );
 }

 deleteFriend(){
  let profile: Profile
  this.profile$.subscribe(p => {
    profile = p;
  }).unsubscribe();
  return this.http.post(this.baseUrl+"user/delete-friend/"+profile.userId, {}).pipe(
    map((response: boolean) => { return response})
  )
 }

}
