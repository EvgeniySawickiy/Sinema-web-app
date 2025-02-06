import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Movie} from '../Interfaces/movie.interface';

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  private apiUrl = 'http://localhost:7000/movies';
  constructor(private http: HttpClient) { }

  getMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(this.apiUrl);
  }

  getMovieById(movieId: string): Observable<Movie> {
    return this.http.get<Movie>(`${this.apiUrl}/${movieId}`);
  }

  addMovie(movie: Movie): Observable<Movie> {
    const payload = { ...movie, genres: undefined };
    return this.http.post<Movie>(`${this.apiUrl}`, payload);
  }


  updateMovie(movie: Movie): Observable<Movie> {
    const payload = { ...movie, genres: undefined };
    return this.http.put<Movie>(`${this.apiUrl}/${movie.id}`, payload);
  }


  deleteMovie(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
