import { Routes } from '@angular/router';
import {LayoutComponent} from './common-ui/layout/layout.component';
import {LoginPageComponent} from './pages/login-page/login-page.component';
import {RegisterPageComponent} from './pages/register-page/register-page.component';
import {ProfilePageComponent} from './pages/profile-page/profile-page.component';
import {HomePageComponent} from './pages/home-page/home-page.component';
import {ConfirmEmailComponent} from './common-ui/confirm-email/confirm-email.component';
import {ResetPasswordComponent} from './common-ui/reset-password/reset-password.component';
import {RequestPasswordComponent} from './common-ui/request-password/request-password.component';
import {MoviePageInfoComponent} from './pages/movie-page-info/movie-page-info.component';
import {AboutUsComponent} from './pages/about-us/about-us.component';
import {MoviesPageComponent} from './pages/movies-page/movies-page.component';

export const routes: Routes = [
  {path:"",
    component: LayoutComponent,
    children: [
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      {path:"home", component: HomePageComponent},
      {path:"login", component: LoginPageComponent},
      {path: 'register', component: RegisterPageComponent },
      {path: 'profile', component: ProfilePageComponent },
      { path: 'confirm-email', component: ConfirmEmailComponent },
      { path: 'request-password-reset', component: RequestPasswordComponent },
      { path: 'reset-password', component: ResetPasswordComponent },
      { path: 'movie/:id', component: MoviePageInfoComponent },
      { path: "about", component: AboutUsComponent},
      { path: "movies", component: MoviesPageComponent },
    ]
  },
  { path: '**', redirectTo: '' },
];
