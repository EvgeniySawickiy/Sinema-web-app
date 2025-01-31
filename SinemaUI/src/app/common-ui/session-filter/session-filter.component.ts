import {Component, EventEmitter, Output} from '@angular/core';
import {NgForOf, NgIf} from '@angular/common';
import {HallService} from '../../data/services/hall.service';
import {MovieService} from '../../data/services/movie.service';
import {GenreService} from '../../data/services/genre.service';

@Component({
  selector: 'app-session-filter',
  imports: [
    NgForOf,
    NgIf
  ],
  templateUrl: './session-filter.component.html',
  styleUrl: './session-filter.component.scss'
})
export class SessionFilterComponent {
  @Output() filterChanged = new EventEmitter<{ time: string, hall: string, genre: string }>();

  selectedTime: string = '';
  selectedHall: string = '';
  selectedGenre: string = '';
  filterVisible: boolean = true;

  timeOptions = ['Утренние', 'Дневные', 'Вечерние', 'Ночные'];
  hallOptions: string[] = [];
  genreOptions: string[] = [];

  constructor(private hallService: HallService, private genreService: GenreService) {}

  ngOnInit(): void {
    this.loadHalls();
    this.loadGenres();
  }

  loadHalls(): void {
    this.hallService.getHalls().subscribe(
      (halls) => {
        this.hallOptions = halls.map(hall => hall.name);
      },
      (error) => console.error('Ошибка загрузки залов:', error)
    );
  }

  loadGenres(): void {
    this.genreService.getGenres().subscribe(
      (genres) => {
        this.genreOptions = genres.map(genre => genre.name);
      },
      (error) => console.error('Ошибка загрузки жанров:', error)
    );
  }

  selectFilter(type: 'time' | 'hall' | 'genre', value: string) {
    if (type === 'time') {
      this.selectedTime = this.selectedTime === value ? '' : value;
    } else if (type === 'hall') {
      this.selectedHall = this.selectedHall === value ? '' : value;
    } else if (type === 'genre') {
      this.selectedGenre = this.selectedGenre === value ? '' : value;
    }

    this.filterChanged.emit({
      time: this.selectedTime,
      hall: this.selectedHall,
      genre: this.selectedGenre
    });
  }

  toggleFilters() {
    this.filterVisible = !this.filterVisible;
  }
}
