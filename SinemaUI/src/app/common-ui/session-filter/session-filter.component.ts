import {Component, EventEmitter, Output} from '@angular/core';
import {NgForOf, NgIf} from '@angular/common';
import {HallService} from '../../data/services/hall.service';
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
  @Output() filterChanged = new EventEmitter<{ time: string, hall: string, genres: string[] }>();

  selectedTime: string = '';
  selectedHall: string = '';
  selectedGenres: string[] = [];
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
    }else if (type === 'genre') {
      if (this.selectedGenres.includes(value)) {
        this.selectedGenres = this.selectedGenres.filter(genre => genre !== value);
      } else {
        this.selectedGenres.push(value);
      }
    }

    this.filterChanged.emit({
      time: this.selectedTime,
      hall: this.selectedHall,
      genres: this.selectedGenres
    });
  }

  toggleFilters() {
    this.filterVisible = !this.filterVisible;
  }
}
