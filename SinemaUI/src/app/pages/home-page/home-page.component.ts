import { Component} from '@angular/core';
import {ShowTime} from '../../data/Interfaces/showtime.interface';
import {Movie} from '../../data/Interfaces/movie.interface';
import {Hall} from '../../data/Interfaces/hall.interface';
import {SessionCardComponent} from '../../cards/session-card/session-card.component';
import {ShowtimeService} from '../../data/services/showtime.service';
import {MovieService} from '../../data/services/movie.service';
import {HallService} from '../../data/services/hall.service';
import {JsonPipe} from '@angular/common';

@Component({
  selector: 'app-home-page',
  imports: [
    SessionCardComponent,
    JsonPipe
  ],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent {
  selectedDate: string = new Date().toISOString().split('T')[0]; // Сегодняшняя дата

  showTimes: ShowTime[] = [];
  movies: Movie[] = [];
  halls: Hall[] = [];
  filteredShowTimes: ShowTime[] = [];

  constructor(
    private showTimeService: ShowtimeService,
    private movieService: MovieService,
    private hallService: HallService
  ) {}
  ngOnInit(): void {
    this.fetchData();
  }

  fetchData() {
    // Загружаем фильмы
    this.movieService.getMovies().subscribe(
      (data) => {
        console.log('Movies loaded:', data);
        this.movies = data;
      },
      (error) => {
        console.error('Error loading movies:', error);
      }
    );

    // Загружаем сеансы
    this.showTimeService.getShowTimes().subscribe(
      (data) => {
        console.log('ShowTimes loaded:', data);
        this.showTimes = data;
      },
      (error) => {
        console.error('Error loading showtimes:', error);
      }
    );

    // Загружаем залы
    this.hallService.getHalls().subscribe(
      (data) => {
        console.log('Halls loaded:', data);
        this.halls = data;
      },
      (error) => {
        console.error('Error loading halls:', error);
      }
    );
  }

  filterShowTimes() {
    // Фильтруем сеансы по выбранной дате
    this.filteredShowTimes = this.showTimes.filter(
      (showTime) =>
        new Date(showTime.startTime).toISOString().split('T')[0] ===
        this.selectedDate
    );
  }

  onDateChange() {
    this.filterShowTimes();
  }
}
