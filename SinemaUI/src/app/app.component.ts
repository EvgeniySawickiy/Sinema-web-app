import {Component, OnInit} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {NotificationService} from './data/services/notification.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  title = 'SinemaUI';

  constructor(private notificationService: NotificationService) {}

  ngOnInit() {
    setTimeout(() => {
      this.notificationService.startConnection();
    }, 1000);
  }
}
