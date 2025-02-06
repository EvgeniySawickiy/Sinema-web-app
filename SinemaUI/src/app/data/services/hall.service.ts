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

  addHall(hall: Partial<Hall>): Observable<Hall> {
    return this.http.post<Hall>(`${this.apiUrl}`, hall);
  }

  updateHall(id: string, hall: Partial<Hall>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, hall);
  }

  deleteHall(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
