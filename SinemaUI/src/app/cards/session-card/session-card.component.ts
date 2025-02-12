import {Component, Input, SimpleChanges} from '@angular/core';
import {ShowTime} from '../../data/Interfaces/showtime.interface';
import {Movie} from '../../data/Interfaces/movie.interface';
import {Hall} from '../../data/Interfaces/hall.interface';
import {RouterLink} from '@angular/router';
import {SessionButtonComponent} from '../../common-ui/session-button/session-button.component';
import {NgFor, NgIf} from '@angular/common';
import {SeatSelectionComponent} from '../../common-ui/seat-selection/seat-selection.component';

@Component({
  selector: 'app-sessions-card',
  standalone: true,
  imports: [
    NgFor,
    RouterLink,
    SessionButtonComponent,
    SeatSelectionComponent,
    NgIf,
  ],
  templateUrl: './session-card.component.html',
  styleUrl: './session-card.component.scss'
})
export class SessionCardComponent {
  @Input() showTimes: ShowTime[] = [];
  @Input() movies: Movie[] = [];
  @Input() halls: Hall[] = [];

  groupedSessions: { movie: Movie; sessions: ShowTime[] }[] = [];

  selectedSession: ShowTime | null = null;
  selectedHall!: Hall ;
  isSeatSelectionOpen = false;


  ngOnInit(): void {
    this.groupSessionsByMovie();
  }

  groupSessionsByMovie() {
    if (this.movies.length === 0 || this.showTimes.length === 0) {
      this.groupedSessions = [];
      return;
    }

    this.groupedSessions = this.movies
      .map((movie) => ({
        movie,
        sessions: this.showTimes.filter((showTime) => showTime.movieId === movie.id),
      }))
      .filter((group) => group.sessions.length > 0);
  }

  onSessionClick(session: ShowTime) {
    this.selectedSession = session;
    this.selectedHall = this.halls.find(hall=>hall.id === session.hallId) as Hall;
    this.isSeatSelectionOpen = true;
  }

  closeSeatSelection() {
    this.isSeatSelectionOpen = false;
  }
}
