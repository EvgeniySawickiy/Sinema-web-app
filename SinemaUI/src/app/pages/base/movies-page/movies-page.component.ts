import { Component } from '@angular/core';
import {NgForOf, NgIf} from '@angular/common';
import {RouterLink} from '@angular/router';
import {Movie} from '../../../data/Interfaces/movie.interface';
import { ShowTime } from '../../../data/Interfaces/showtime.interface';
import {MovieService} from '../../../data/services/movie.service';
import {ShowtimeService} from '../../../data/services/showtime.service';

@Component({
  selector: 'app-movies-page',
  imports: [
    NgIf,
    NgForOf,
    RouterLink
  ],
  templateUrl: './movies-page.component.html',
  styleUrl: './movies-page.component.scss'
})
export class MoviesPageComponent {
  movies: Movie[] = [];
  showTimes: ShowTime[] = [];
  loading = true;
  error: string | null = null;

  constructor(private movieService: MovieService, private showtimeService: ShowtimeService) {}

  ngOnInit(): void {
    this.loadMoviesWithValidShowtimes();
  }

  loadMoviesWithValidShowtimes(): void {
    this.movieService.getMovies().subscribe(
      (movies) => {
        this.showtimeService.getShowTimes().subscribe(
          (showtimes) => {
            this.showTimes = showtimes.filter(st => new Date(st.startTime) > new Date());
            const validMovieIds = new Set(this.showTimes.map(st => st.movieId));
            this.movies = movies.filter(movie => validMovieIds.has(movie.id));
            this.loading = false;
          },
          (error) => {
            this.error = 'Ошибка загрузки сеансов.';
            console.error(error);
            this.loading = false;
          }
        );
      },
      (error) => {
        this.error = 'Ошибка загрузки фильмов.';
        console.error(error);
        this.loading = false;
      }
    );
  }
}
