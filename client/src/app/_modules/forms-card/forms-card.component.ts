import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-forms-card',
  templateUrl: './forms-card.component.html',
  styleUrls: ['./forms-card.component.scss']
})
export class FormsCardComponent implements OnInit {
  flipped: boolean = false;
  constructor() { }

  ngOnInit(): void {
  }

}
