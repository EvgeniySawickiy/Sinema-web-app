import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {Hall} from '../Interfaces/hall.interface';
import {Genre} from '../Interfaces/genre.interface';

@Injectable({
  providedIn: 'root'
})
export class GenreService {

  private baseUrl = 'http://localhost:7000/genres';

  constructor(private http: HttpClient) {}

  getGenres(): Observable<Genre[]> {
    return this.http.get<Genre[]>(`${this.baseUrl}`);
  }
}
