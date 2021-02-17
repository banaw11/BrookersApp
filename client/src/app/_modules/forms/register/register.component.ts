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
    email: "",
    password: "",
    rePassword: "",
    knownAs: "",
  };

  statusMessage? : boolean = true;
  submitted: boolean = false;
  message?: string;
  errors: string[] = [];
  constructor(private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
  }

  register(): void{
    this.accountService.register(this.user).subscribe( response => {
      this.router.navigateByUrl('/home');
      this.submitted=true;
    }, error => {
      this.errors = error;
      if(this.errors.length>0){
        this.statusMessage=false;
        this.message="Register unsuccessful";
      }
      else{
        this.statusMessage=true;
        this.message="Register successful";
      }
    })
  }
}
