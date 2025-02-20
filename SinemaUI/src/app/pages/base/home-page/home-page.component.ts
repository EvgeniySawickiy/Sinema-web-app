import {ChangeDetectorRef, Component} from '@angular/core';
import {ShowTime} from '../../../data/Interfaces/showtime.interface';
import {Movie} from '../../../data/Interfaces/movie.interface';
import {Hall} from '../../../data/Interfaces/hall.interface';
import {SessionCardComponent} from '../../../cards/session-card/session-card.component';
import {ShowtimeService} from '../../../data/services/showtime.service';
import {MovieService} from '../../../data/services/movie.service';
import {HallService} from '../../../data/services/hall.service';
import {NgIf} from '@angular/common';
import {DateFilterComponent} from '../../../common-ui/date-filter/date-filter.component';
import {SessionFilterComponent} from '../../../common-ui/session-filter/session-filter.component';

@Component({
  selector: 'app-home-page',
  imports: [
    SessionCardComponent,
    DateFilterComponent,
    NgIf,
    SessionFilterComponent,
  ],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent {
  showTimes: ShowTime[] = [];
  movies: Movie[] = [];
  halls: Hall[] = [];
  filteredByDateShowTimes: ShowTime[] = [];
  filteredShowTimes: ShowTime[] = [];

  selectedFilters: { time: string; hall: string; genres: string[] } = { time: '', hall: '', genres: [] };

  constructor(
    private showTimeService: ShowtimeService,
    private movieService: MovieService,
    private hallService: HallService,
    private cdr: ChangeDetectorRef
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
      this.onDateSelected(new Date());
    }
  }

  onDateSelected(selectedDate: Date): void {
    this.filteredByDateShowTimes = this.showTimes.filter((showTime) => {
      const showDate = new Date(showTime.startTime);
      return (
        showDate.getFullYear() === selectedDate.getFullYear() &&
        showDate.getMonth() === selectedDate.getMonth() &&
        showDate.getDate() === selectedDate.getDate()
      );
    });

    this.filteredShowTimes= this.filteredByDateShowTimes;
  }

  applyFilters() {
    this.filteredShowTimes = [...this.filteredByDateShowTimes.filter((session) => {
      const matchesTime =
        !this.selectedFilters.time || this.filterByTime(session.startTime);
      const matchesHall =
        !this.selectedFilters.hall || session.hallName === this.selectedFilters.hall;

      const movie = this.movies.find((m) => m.id === session.movieId);
      const movieGenres = movie?.genres ?? [];

      const matchesGenres =
        this.selectedFilters.genres.length === 0 ||
        this.selectedFilters.genres.every(genre => movieGenres.includes(genre));

      return matchesTime && matchesHall && matchesGenres;
    })];

    this.cdr.detectChanges();
  }

  onFilterChanged(filters: { time: string; hall: string; genres: string[] }) {
    this.selectedFilters = filters;
    this.applyFilters();
  }

  filterByTime(startTime: string): boolean {
    const sessionHour = new Date(startTime).getHours();
    if (this.selectedFilters.time === 'Утренние') return sessionHour >= 6 && sessionHour < 12;
    if (this.selectedFilters.time === 'Дневные') return sessionHour >= 12 && sessionHour < 18;
    if (this.selectedFilters.time === 'Вечерние') return sessionHour >= 18 && sessionHour < 23;
    if (this.selectedFilters.time === 'Ночные') return sessionHour >= 23 || sessionHour < 6;
    return true;
  }
}
