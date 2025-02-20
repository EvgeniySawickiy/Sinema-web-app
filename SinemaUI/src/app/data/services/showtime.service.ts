import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {ShowTime} from '../Interfaces/showtime.interface';

@Injectable({
  providedIn: 'root'
})
export class ShowtimeService {

  private apiUrl = 'http://localhost:7000/showtimes';
  constructor(private http: HttpClient) { }

  getShowTimes(): Observable<ShowTime[]> {
    return this.http.get<ShowTime[]>(this.apiUrl);
  }

  addShowtime(showtime: Partial<ShowTime>): Observable<ShowTime> {
    return this.http.post<ShowTime>(this.apiUrl, showtime);
  }

  updateShowtime(id: string, showtime: Partial<ShowTime>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, showtime);
  }

  deleteShowtime(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
