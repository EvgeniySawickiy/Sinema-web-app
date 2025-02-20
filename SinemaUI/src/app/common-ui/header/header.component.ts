import { Component } from '@angular/core';
import {RouterLink} from '@angular/router';
import {AuthService} from '../../auth/auth.service';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-header',
  imports: [
    RouterLink,
    NgIf
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  isAuthenticated = false;

  constructor(protected authService: AuthService) {}

  ngOnInit(): void {
    this.authService.isAuthenticated$.subscribe(authStatus => {
      this.isAuthenticated = authStatus;
    });
  }

  logout(): void {
    this.authService.logout();
  }
}
