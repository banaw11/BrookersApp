import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { UsersService } from 'src/app/services/users.service';
import { Profile } from 'src/app/_models/profile';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-profile-details',
  templateUrl: './profile-details.component.html',
  styleUrls: ['./profile-details.component.scss']
})
export class ProfileDetailsComponent implements OnInit {
  private profileSource = new BehaviorSubject<Profile>(null);
  profile = this.profileSource.asObservable();
  constructor(private usersService: UsersService, private route : ActivatedRoute) { 
    console.log("ref");
  }

  ngOnInit(): void {
    this.usersService.getProfile(this.route.snapshot.paramMap.get('userName'))
      .pipe(take(1)).subscribe((profile : Profile) => {
        this.profileSource.next(profile);
    })
    
  }

}
