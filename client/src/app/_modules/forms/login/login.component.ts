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
    this.submitted=true;
    this.errors =[];
   this.accountService.login(this.user).subscribe(response => {
    this.statusMessage=true;
    this.message="Login successful";
    setTimeout(() => {
      this.router.navigateByUrl('/home');
    }, 500);
  }, error => {
    this.errors.push(error.error);
    this.statusMessage=false;
    this.message="Login unsuccesful";
   })
  }

}
