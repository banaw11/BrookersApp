import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { Account } from 'src/app/_models/account';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  user: Account = {
    email: "",
    password: "",
  };

  statusMessage? : boolean = true;
  submitted: boolean = false;
  message?: string;
  errors: string[] = [];
  constructor(private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
  }

  login(): void {
   this.accountService.login(this.user).subscribe(response => {
    this.router.navigateByUrl('/home');
    this.submitted=true;
  }, error => {
    this.errors = error;
    if(this.errors.length>0){
      this.statusMessage=false;
      this.message="Login unsuccessful";
    }
    else{
      this.statusMessage=true;
      this.message="Login successful";
    }
   })
  }

}
