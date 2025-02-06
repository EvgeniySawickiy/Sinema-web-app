import {Component, ElementRef, ViewChild} from '@angular/core';
import {ShowTime} from '../../data/Interfaces/showtime.interface';
import {AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {ShowtimeService} from '../../data/services/showtime.service';
import {MovieService} from '../../data/services/movie.service';
import {HallService} from '../../data/services/hall.service';
import {Movie} from '../../data/Interfaces/movie.interface';
import {Hall} from '../../data/Interfaces/hall.interface';
import {DatePipe, NgForOf, NgIf} from '@angular/common';

@Component({
  selector: 'app-admin-showtimes',
  imports: [
    ReactiveFormsModule,
    NgIf,
    NgForOf,
    DatePipe
  ],
  templateUrl: './admin-showtimes.component.html',
  styleUrl: './admin-showtimes.component.scss'
})
export class AdminShowtimesComponent {
  @ViewChild('showtimeFormElement') showtimeFormElement!: ElementRef;

  showtimeForm: FormGroup;
  showtimes: ShowTime[] = [];
  groupedShowtimes: { movie: Movie; sessions: ShowTime[] }[] = [];
  movies: Movie[] = [];
  halls: Hall[] = [];
  isEditing = false;
  editingShowtimeId: string | null = null;

  constructor(
    private fb: FormBuilder,
    private showtimeService: ShowtimeService,
    private movieService: MovieService,
    private hallService: HallService
  ) {
    this.showtimeForm = this.fb.group({
      movieId: ['', Validators.required],
      hallId: ['', Validators.required],
      startTime: ['', [Validators.required, this.futureDateValidator]],
      ticketPrice: [null, [Validators.required, Validators.min(1)]],
    });

  }

  futureDateValidator(control: AbstractControl) {
    const selectedDate = new Date(control.value);
    const now = new Date();

    return selectedDate > now ? null : { pastDate: true };
  }


  ngOnInit(): void {
    this.fetchData();
  }

  fetchData() {
    this.movieService.getMovies().subscribe(movies => {
      this.movies = movies;
      this.loadShowtimes();
    });

    this.hallService.getHalls().subscribe(halls => {
      this.halls = halls;
    });
  }

  loadShowtimes() {
    this.showtimeService.getShowTimes().subscribe(showtimes => {
      // Преобразуем дату из UTC в локальное время
      this.showtimes = showtimes.map(st => ({
        ...st,
        startTime: new Date(st.startTime).toISOString()
      }));

      this.groupSessionsByMovie();
    });
  }

  groupSessionsByMovie() {
    if (!this.movies.length || !this.showtimes.length) return;

    this.groupedShowtimes = this.movies
      .map(movie => ({
        movie,
        sessions: this.showtimes
          .filter(st => st.movieId === movie.id)
          .sort((a, b) => new Date(a.startTime).getTime() - new Date(b.startTime).getTime())
      }))
      .filter(group => group.sessions.length > 0);
  }

  getHallName(hallId: string): string {
    return this.halls.find(h => h.id === hallId)?.name || 'Неизвестно';
  }

  addShowtime() {
    if (this.showtimeForm.invalid) return;

    const formData = this.showtimeForm.value;
    const showtime: ShowTime = {
      ...formData,
      startTime: new Date(formData.startTime).toISOString()
    };

    this.showtimeService.addShowtime(showtime).subscribe(() => {
      this.loadShowtimes();
      this.showtimeForm.reset();
    });
  }

  editShowtime(showtime: ShowTime) {
    this.isEditing = true;
    this.editingShowtimeId = showtime.id;

    this.showtimeForm.patchValue({
      movieId: showtime.movieId,
      hallId: showtime.hallId,
      startTime: new Date(showtime.startTime).toISOString().slice(0, 16),
      ticketPrice: showtime.ticketPrice
    });

    setTimeout(() => {
      this.showtimeFormElement.nativeElement.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }, 100);
  }

  updateShowtime() {
    if (this.showtimeForm.invalid || !this.editingShowtimeId) return;

    const formData = this.showtimeForm.value;
    const updatedShowtime: ShowTime = {
      id: this.editingShowtimeId,
      ...formData,
      startTime: new Date(formData.startTime).toISOString() // Преобразуем в UTC
    };

    this.showtimeService.updateShowtime(this.editingShowtimeId!, updatedShowtime).subscribe(() => {
      this.isEditing = false;
      this.editingShowtimeId = null;
      this.loadShowtimes();
      this.showtimeForm.reset();
    });
  }

  deleteShowtime(id: string) {
    this.showtimeService.deleteShowtime(id).subscribe(() => {
      this.loadShowtimes();
    });
  }

  cancelEdit() {
    this.isEditing = false;
    this.editingShowtimeId = null;
    this.showtimeForm.reset();
  }
}
