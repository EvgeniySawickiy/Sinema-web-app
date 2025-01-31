import { Injectable } from '@angular/core';
import {Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {Hall} from '../Interfaces/hall.interface';

@Injectable({
  providedIn: 'root'
})
export class HallService {
  private apiUrl = 'http://localhost:7000/halls';
  constructor(private http: HttpClient) { }

  getHalls(): Observable<Hall[]> {
    return this.http.get<Hall[]>(this.apiUrl);
  }

  getHallById(hallId : string): Observable<Hall> {
    return this.http.get<Hall>(`${this.apiUrl}/${hallId}`);
  }
}
