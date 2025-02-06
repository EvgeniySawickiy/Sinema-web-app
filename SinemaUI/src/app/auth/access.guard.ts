import {inject} from '@angular/core';
import {AuthService} from './auth.service';
import {Router} from 'express';

export const canActivateAuth =() =>{
  const isLoggedIn = inject(AuthService).isAuthenticated;

  if (isLoggedIn){
    return true;
  }

    return inject(Router).createUrlTree('/login');
}
