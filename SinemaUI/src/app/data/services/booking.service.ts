import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {forkJoin, map, Observable, switchMap, throwError} from 'rxjs';
import {ShowTime} from '../Interfaces/showtime.interface';
import {Booking, BookingRequest} from '../Interfaces/booking.interface';
import {Movie} from '../Interfaces/movie.interface';
import {Hall} from '../Interfaces/hall.interface';
import {AuthService} from '../../auth/auth.service';
import {Seat} from '../Interfaces/seat.interface';

@Injectable({
  providedIn: 'root'
})

export class BookingService {
  private apiUrl = 'http://localhost:7000/bookings';
  private showtimesUrl = 'http://localhost:7000/showtimes';
  private moviesUrl = 'http://localhost:7000/movies';
  private hallsUrl = 'http://localhost:7000/halls';

  constructor(private http: HttpClient, private authService: AuthService) {}

  getReservedSeats(showtimeId: string): Observable<Seat[]> {
    return this.http.get<Seat[]>(`${this.apiUrl}/seats/showtime/${showtimeId}`);
  }
  bookSeats(bookingData: BookingRequest): Observable<any> {
    return this.http.post<any>(this.apiUrl, bookingData);
  }
  getUserBookings(): Observable<any[]> {
    const userId = this.authService.getUserId();
    if (!userId) {
      console.error('Ошибка: userId не найден');
      return throwError(() => new Error('User ID not found'));
    }

    return this.http.get<Booking[]>(`${this.apiUrl}/user/${userId}`).pipe(
      switchMap((bookings) => {
        if (!bookings.length) {
          return throwError(() => new Error('Нет бронирований.'));
        }

        const requests = bookings.map((booking) =>
          this.http.get<ShowTime>(`${this.showtimesUrl}/${booking.showtimeId}`).pipe(
            switchMap((showtime) =>
              forkJoin({
                movie: this.http.get<Movie>(`${this.moviesUrl}/${showtime.movieId}`),
                hall: this.http.get<Hall>(`${this.hallsUrl}/${showtime.hallId}`)
              }).pipe(
                map(({ movie, hall }) => ({
                  id: booking.id,
                  userId: booking.userId,
                  showtimeId: booking.showtimeId,
                  movieTitle: movie.title,
                  movieImage: movie.imageUrl,
                  hallName: hall.name,
                  startTime: showtime.startTime,
                  ticketPrice: showtime.ticketPrice,
                  status: booking.status,
                  seats: booking.seats.map(seat => `Ряд ${seat.row}, Место ${seat.number}`),
                  totalAmount: booking.totalAmount
                }))
              )
            )
          )
        );

        return forkJoin(requests);
      })
    );
  }

  cancelBooking(bookingId: string, reason: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${bookingId}`, {
      body: { reason },
    });
  }
}
