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
export class ProfileComponent implements OnInit {

  ngOnInit(): void {
  }

}
