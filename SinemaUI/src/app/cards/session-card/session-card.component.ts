import {Component, Input, Output, SimpleChanges} from '@angular/core';
import {ShowTime} from '../../data/Interfaces/showtime.interface';
import {Movie} from '../../data/Interfaces/movie.interface';
import {Hall} from '../../data/Interfaces/hall.interface';
import {DatePipe, JsonPipe, NgFor, NgIf} from '@angular/common';


@Component({
  selector: 'app-sessions-card',
  standalone: true,
  imports: [
    NgFor,
    DatePipe,
    NgIf,
    JsonPipe,
  ],
  templateUrl: './session-card.component.html',
  styleUrl: './session-card.component.scss'
})
export class SessionCardComponent {
  @Input() showTimes: ShowTime[] = [];
  @Input() movies: Movie[] = [];
  @Input() halls: Hall[] = [];

  groupedSessions: { movie: Movie; sessions: ShowTime[] }[] = [];

  ngOnInit(): void {
    this.groupSessionsByMovie();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['showTimes'] || changes['movies']) {
      this.groupSessionsByMovie();
    }
  }
  groupSessionsByMovie() {
    if (this.movies.length === 0 || this.showTimes.length === 0) {
      this.groupedSessions = [];
      return;
    }

    this.groupedSessions = this.movies.map((movie) => ({
      movie,
      sessions: this.showTimes.filter((showTime) => showTime.movieId === movie.id),
    }));
  }


  onSessionClick(session: ShowTime) {
    alert(
      `Вы выбрали сеанс:\nФильм: ${session.movieTitle}\nЗал: ${session.hallName}\nВремя: ${new Date(
        session.startTime
      ).toLocaleTimeString()}`
    );
  }
}
