import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { profile } from 'console';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { UsersService } from 'src/app/services/users.service';
import { Profile } from 'src/app/_models/profile';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit, OnDestroy {
 private profileSource = new BehaviorSubject<Profile>(null);
 profile$ = this.profileSource.asObservable();

  constructor(private route: ActivatedRoute, private usersService: UsersService) {
    this.usersService.getProfile(this.route.snapshot.paramMap.get('userName'))
      .pipe(take(1)).subscribe((profile : Profile) => {
        this.profileSource.next(profile);
    })
   }
  ngOnDestroy(): void {
    this.profileSource.next(null);
  }

  ngOnInit(): void {
  }

}
