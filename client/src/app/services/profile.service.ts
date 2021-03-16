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
}
