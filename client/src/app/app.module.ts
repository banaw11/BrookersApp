import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NbThemeModule, NbLayoutModule, NbSelectModule, NbActionsModule, NbTabsetModule, NbCardModule, NbButtonModule, NbInputModule, NbAlertModule,
 NbIconModule } from '@nebular/theme';
import { NbEvaIconsModule } from '@nebular/eva-icons';
import { HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './components/home/home.component';
import { ChangeThemeComponent } from './_modules/change-theme/change-theme.component';
import { WelcomeComponent } from './components/welcome/welcome.component';
import { NavBarComponent } from './components/nav/nav-bar/nav-bar.component';
import { NavMenuComponent } from './components/nav/nav-menu/nav-menu.component';
import { LoginComponent } from './_modules/forms/login/login.component';
import { RegisterComponent } from './_modules/forms/register/register.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ChangeThemeComponent,
    WelcomeComponent,
    NavBarComponent,
    NavMenuComponent,
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    NbThemeModule.forRoot({ name: 'dark' }),
    NbLayoutModule,
    NbEvaIconsModule,
    NbSelectModule,
    FlexLayoutModule,
    FormsModule,
    ReactiveFormsModule,
    NbActionsModule,
    NbTabsetModule,
    NbCardModule,
    NbButtonModule,
    NbInputModule,
    NbAlertModule,
    NbIconModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
