import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { Account } from 'src/app/_models/account';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  user: Account = {
    userName: "",
    email: "",
    password: "",
    rePassword: ""
  };

  statusMessage? : boolean = true;
  submitted: boolean = false;
  message?: string;
  errors: any[] = [];
  constructor(private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
  }

  register(): void{
    this.submitted=true;
    this.accountService.register(this.user).subscribe( response => {
      this.statusMessage=true;
      this.message="Register successful";
      setTimeout(()=>{
        this.router.navigateByUrl('/home');
      },500)
    }, error => {
      this.errors = error.error;
      this.statusMessage=false;
      this.message="Register unsuccessful";
    })
  }
}
