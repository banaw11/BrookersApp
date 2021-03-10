import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { map } from 'rxjs/operators'
import { UsersService } from './users.service';
import { PressenceService } from './pressence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl=environment.apiUrl;
  constructor(private http: HttpClient, private usersService: UsersService, private pressenceService: PressenceService) { }

  register(model: any){
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((user: User) => {
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.usersService.setCurentUser(user);
          this.pressenceService.createHubConnection(user);
        }
        return user;
      })
    )
  }

  login(model: any){
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response
        if(user){
          this.usersService.setCurentUser(user);
          localStorage.setItem('user', JSON.stringify(user));
          this.pressenceService.createHubConnection(user);
        }
      })
    )
  }

  logout(){
    localStorage.removeItem('user');
    this.usersService.logoutUser();
    this.pressenceService.stopHubConnection();
  }
}
