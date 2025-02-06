import {Component, inject} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {HeaderComponent} from '../header/header.component';
import {ProfileService} from '../../data/services/profile.service';

@Component({
  selector: 'app-layout',
  imports: [
    RouterOutlet,
    HeaderComponent
  ],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {
profileService =inject(ProfileService)

  ngOnInit() {
  console.log('Component initialized');
  }
}
