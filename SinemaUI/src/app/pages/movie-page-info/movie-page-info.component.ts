import { Component } from '@angular/core';
import {DatePipe, NgForOf, NgIf} from '@angular/common';
import {Movie} from '../../data/Interfaces/movie.interface';
import {ActivatedRoute} from '@angular/router';
import {MovieService} from '../../data/services/movie.service';
import {ShowTime} from '../../data/Interfaces/showtime.interface';
import {ShowtimeService} from '../../data/services/showtime.service';
import {SessionButtonComponent} from '../../common-ui/session-button/session-button.component';
import {DomSanitizer, SafeResourceUrl} from '@angular/platform-browser';
import {Hall} from '../../data/Interfaces/hall.interface';
import {HallService} from '../../data/services/hall.service';
import {SeatSelectionComponent} from '../../common-ui/seat-selection/seat-selection.component';

@Component({
  selector: 'app-movie-page-info',
  imports: [
    NgIf,
    DatePipe,
    NgForOf,
    SessionButtonComponent,
    SeatSelectionComponent
  ],
  templateUrl: './movie-page-info.component.html',
  styleUrl: './movie-page-info.component.scss'
})
export class MoviePageInfoComponent {
  movie: Movie | null = null;
  showTimes: ShowTime[] = [];
  groupedShowTimes: { [date: string]: ShowTime[] } = {};
  safeTrailerUrl: SafeResourceUrl | null = null;
  loading: boolean = true;
  error: string | null = null;

  selectedSession: ShowTime | null = null;
  selectedHall!: Hall ;
  isSeatSelectionOpen = false;

  constructor(
    private route: ActivatedRoute,
    private movieService: MovieService,
    private hallService: HallService,
    private showtimeService: ShowtimeService,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    const movieId = this.route.snapshot.paramMap.get('id');
    if (movieId) {

      this.movieService.getMovieById(movieId).subscribe(
        (data) => {
          this.movie = data;
          this.loading = false;

          if (this.movie.trailerUrl) {
            const formattedUrl = this.formatTrailerUrl(this.movie.trailerUrl);
            this.safeTrailerUrl = this.sanitizer.bypassSecurityTrustResourceUrl(formattedUrl);
          }
        },
        (error) => {
          console.error('Ошибка загрузки фильма:', error);
          this.error = 'Ошибка загрузки фильма';
          this.loading = false;
        }
      );

      this.showtimeService.getShowTimes().subscribe(
        (data) => {
          this.showTimes = data.filter((show) => show.movieId === movieId);
          this.groupShowTimesByDate();
        },
        (error) => {
          console.error('Ошибка загрузки сеансов:', error);
        }
      );
    }
  }

  groupShowTimesByDate() {
    const today = new Date().toISOString().split('T')[0];
    this.groupedShowTimes = this.showTimes.reduce((acc, session) => {
      const date = session.startTime.split('T')[0];
      if (date >= today ){
      if (!acc[date]) {
        acc[date] = [];
      }
      acc[date].push(session);
      }

      return acc;
    }, {} as { [date: string]: ShowTime[] });
  }

  formatTrailerUrl(url: string): string {
    if (url.includes('youtu.be')) {
      return url.replace('youtu.be/', 'www.youtube.com/embed/');
    }
    if (url.includes('watch?v=')) {
      return url.replace('watch?v=', 'embed/');
    }
    return url;
  }

  onSessionClick(session: ShowTime) {
    this.selectedSession = session;

    this.hallService.getHallById(session.hallId).subscribe(
      (data) => {
        this.selectedHall = data;
      },
      (error) => {
        console.error('Error loading halls:', error);
      }
    );
    this.isSeatSelectionOpen = true;
  }

  closeSeatSelection() {
    this.isSeatSelectionOpen = false;
  }

  protected readonly Object = Object;
}
