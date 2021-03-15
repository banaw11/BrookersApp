import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.scss']
})
export class WelcomeComponent implements OnInit {

  constructor( private accountService: AccountService) { }

  ngOnInit(): void {
    if(this.checkLocalStorage){
      this.accountService.logout();
    }
  }

  checkLocalStorage(){
    return localStorage.getItem('user') ? true: false;
  }
}
