import { Component } from '@angular/core';
import {Movie} from '../../data/Interfaces/movie.interface';
import { ShowTime } from '../../data/Interfaces/showtime.interface';
import {Hall} from '../../data/Interfaces/hall.interface';
import {Seat} from '../../data/Interfaces/seat.interface';
import {ShowtimeService} from '../../data/services/showtime.service';
import {BookingService} from '../../data/services/booking.service';
import {MovieService} from '../../data/services/movie.service';
import {HallService} from '../../data/services/hall.service';
import {DatePipe, NgForOf, NgIf} from '@angular/common';

@Component({
  selector: 'app-admin-bookings',
  imports: [
    DatePipe,
    NgIf,
    NgForOf
  ],
  templateUrl: './admin-bookings.component.html',
  styleUrl: './admin-bookings.component.scss'
})
export class AdminBookingsComponent {
  movies: Movie[] = [];
  showtimes: ShowTime[] = [];
  halls: Hall[] = [];
  groupedShowtimes: { movie: Movie; sessions: ShowTime[] }[] = [];
  selectedShowtime: ShowTime | null = null;
  bookedSeats: Seat[] = [];
  expandedMovieId: string | null = null;

  constructor(
    private showtimeService: ShowtimeService,
    private bookingService: BookingService,
    private movieService: MovieService,
    private hallService: HallService
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.movieService.getMovies().subscribe(movies => {
      this.movies = movies;
      this.loadShowtimes();
    });

    this.hallService.getHalls().subscribe(halls => {
      this.halls = halls;
    });
  }

  loadShowtimes() {
    this.showtimeService.getShowTimes().subscribe(showtimes => {
      this.showtimes = showtimes;
      this.groupSessionsByMovie();
    });
  }

  groupSessionsByMovie() {
    if (!this.movies.length || !this.showtimes.length) return;

    this.groupedShowtimes = this.movies
      .map(movie => ({
        movie,
        sessions: this.showtimes
          .filter(st => st.movieId === movie.id)
          .sort((a, b) => new Date(a.startTime).getTime() - new Date(b.startTime).getTime())
      }))
      .filter(group => group.sessions.length > 0);
  }

  getHallName(hallId: string): string {
    return this.halls.find(h => h.id === hallId)?.name || 'Неизвестно';
  }

  loadBookings(showtime: ShowTime) {
    this.selectedShowtime = showtime;
    this.bookingService.getReservedSeats(showtime.id).subscribe(
      (seats) => {
        this.bookedSeats = seats;
      },
      (error) => {
        console.error('Ошибка загрузки забронированных мест:', error);
        this.bookedSeats = [];
      }
    );
  }

  toggleMovie(movieId: string) {
    this.expandedMovieId = this.expandedMovieId === movieId ? null : movieId;
  }
}
