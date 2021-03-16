import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommunityComponent } from './components/community/community.component';
import { HomeComponent } from './components/home/home.component';
import { InvestmentsComponent } from './components/investments/investments.component';
import { ProfileComponent } from './components/profile/profile.component';
import { SettingsComponent } from './components/settings/settings.component';
import { WelcomeComponent } from './components/welcome/welcome.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: WelcomeComponent,
    
  },
  {
    path:'',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'home',
         component: HomeComponent,
        children: [
          {path: 'community', component: CommunityComponent},
          {path: 'investments', component: InvestmentsComponent},
          {path: 'settings', component: SettingsComponent},
          {path: 'profile', component: ProfileComponent},
        ]}
    ]
  },
  {path: '**', component: WelcomeComponent, pathMatch:'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
