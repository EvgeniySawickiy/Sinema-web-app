import { Component} from '@angular/core';
import {ShowTime} from '../../data/Interfaces/showtime.interface';
import {Movie} from '../../data/Interfaces/movie.interface';
import {Hall} from '../../data/Interfaces/hall.interface';
import {SessionCardComponent} from '../../cards/session-card/session-card.component';
import {ShowtimeService} from '../../data/services/showtime.service';
import {MovieService} from '../../data/services/movie.service';
import {HallService} from '../../data/services/hall.service';
import {JsonPipe, NgIf} from '@angular/common';
import {DateFilterComponent} from '../../common-ui/date-filter/date-filter.component';
import {SessionFilterComponent} from '../../common-ui/session-filter/session-filter.component';
import {SeatSelectionComponent} from '../../common-ui/seat-selection/seat-selection.component';

@Component({
  selector: 'app-home-page',
  imports: [
    SessionCardComponent,
    DateFilterComponent,
    NgIf,
    SessionFilterComponent,
    SeatSelectionComponent
  ],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent {
  selectedDate: string = new Date().toISOString().split('T')[0];

  showTimes: ShowTime[] = [];
  movies: Movie[] = [];
  halls: Hall[] = [];
  filteredByDateShowTimes: ShowTime[] = [];
  filteredShowTimes: ShowTime[] = [];

  selectedFilters = { time: '', hall: '', genre: '' };

  isSeatSelectionOpen = false;
  selectedSession!: ShowTime;

  constructor(
    private showTimeService: ShowtimeService,
    private movieService: MovieService,
    private hallService: HallService
  ) {}

  ngOnInit(): void {
    this.fetchData();
  }

  fetchData() {
    let moviesLoaded = false;
    let showTimesLoaded = false;
    let hallsLoaded = false;


    this.movieService.getMovies().subscribe(
      (data) => {
        console.log('Movies loaded:', data);
        this.movies = data;
        moviesLoaded = true;
        this.tryFilterShowTimes(moviesLoaded, showTimesLoaded, hallsLoaded);
      },
      (error) => {
        console.error('Error loading movies:', error);
      }
    );

    this.showTimeService.getShowTimes().subscribe(
      (data) => {
        console.log('ShowTimes loaded:', data);
        this.showTimes = data;
        showTimesLoaded = true;
        this.tryFilterShowTimes(moviesLoaded, showTimesLoaded, hallsLoaded);
      },
      (error) => {
        console.error('Error loading showtimes:', error);
      }
    );

    this.hallService.getHalls().subscribe(
      (data) => {
        console.log('Halls loaded:', data);
        this.halls = data;
        hallsLoaded = true;
        this.tryFilterShowTimes(moviesLoaded, showTimesLoaded, hallsLoaded);
      },
      (error) => {
        console.error('Error loading halls:', error);
      }
    );
  }

  tryFilterShowTimes(moviesLoaded: boolean, showTimesLoaded: boolean, hallsLoaded: boolean) {
    if (moviesLoaded && showTimesLoaded && hallsLoaded) {
      console.log('Все данные загружены, выполняем фильтрацию.');
      this.onDateSelected(new Date());
    }
  }

  onDateSelected(selectedDate: Date): void {
    console.log('Выбранная дата:', selectedDate);

    this.filteredByDateShowTimes = this.showTimes.filter((showTime) => {
      const showDate = new Date(showTime.startTime);
      return (
        showDate.getFullYear() === selectedDate.getFullYear() &&
        showDate.getMonth() === selectedDate.getMonth() &&
        showDate.getDate() === selectedDate.getDate()
      );
    });

    this.filteredShowTimes= this.filteredByDateShowTimes;
    console.log('Отфильтрованные сеансы:', this.filteredByDateShowTimes);
  }

  applyFilters() {
    this.filteredShowTimes = this.filteredByDateShowTimes.filter((session) => {
      const matchesTime =
        !this.selectedFilters.time || this.filterByTime(session.startTime);
      const matchesHall =
        !this.selectedFilters.hall || session.hallName === this.selectedFilters.hall;
      const matchesGenre =
        !this.selectedFilters.genre ||
        this.movies.find((m) => m.id === session.movieId)?.genres.includes(this.selectedFilters.genre);

      return matchesTime && matchesHall && matchesGenre;
    });
    console.log('Отфильтрованные дополнительно сеансы:', this.filteredShowTimes);
  }

  filterByTime(startTime: string): boolean {
    const sessionHour = new Date(startTime).getHours();
    if (this.selectedFilters.time === 'Утренние') return sessionHour >= 6 && sessionHour < 12;
    if (this.selectedFilters.time === 'Дневные') return sessionHour >= 12 && sessionHour < 18;
    if (this.selectedFilters.time === 'Вечерние') return sessionHour >= 18 && sessionHour < 23;
    if (this.selectedFilters.time === 'Ночные') return sessionHour >= 23 || sessionHour < 6;
    return true;
  }

  onFilterChanged(filters: { time: string; hall: string; genre: string }) {

    this.selectedFilters = filters;
    this.applyFilters();
  }

  onSessionClick(session: ShowTime) {
    this.selectedSession = session;
    this.isSeatSelectionOpen = true;
  }

  closeSeatSelection() {
    this.isSeatSelectionOpen = false;
  }
}
