import { Component} from '@angular/core';
import { NbThemeService } from '@nebular/theme';

@Component({
  selector: 'app-change-theme',
  templateUrl: './change-theme.component.html',
  styleUrls: ['./change-theme.component.scss']
})
export class ChangeThemeComponent {

  themeName ="default";
  constructor(private themeService: NbThemeService) { }

  changeTheme(name: string){
    this.themeService.changeTheme(name);
  }
}
