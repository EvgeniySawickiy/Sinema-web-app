import {Component, EventEmitter, Input, Output} from '@angular/core';
import {ShowTime} from '../../data/Interfaces/showtime.interface';
import {DatePipe} from '@angular/common';

@Component({
  selector: 'app-session-button',
  imports: [
    DatePipe
  ],
  templateUrl: './session-button.component.html',
  styleUrl: './session-button.component.scss'
})
export class SessionButtonComponent {
  @Input() session!: ShowTime;
  @Output() sessionClick = new EventEmitter<ShowTime>();

  isPastSession(): boolean {
    const now = new Date();
    const sessionTime = new Date(this.session.startTime);
    return sessionTime < now;
  }
  onClick() {
    if (!this.isPastSession()) {
      this.sessionClick.emit(this.session);
    }
  }
}
