import { Component } from '@angular/core';
import {NgForOf, NgIf} from '@angular/common';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {Movie} from '../../../data/Interfaces/movie.interface';
import {Genre} from '../../../data/Interfaces/genre.interface';
import {MovieService} from '../../../data/services/movie.service';
import {GenreService} from '../../../data/services/genre.service';

@Component({
  selector: 'app-admin-movies',
  imports: [
    NgIf,
    FormsModule,
    NgForOf,
    ReactiveFormsModule
  ],
  templateUrl: './admin-movies.component.html',
  styleUrl: './admin-movies.component.scss'
})
export class AdminMoviesComponent {
  movies: Movie[] = [];
  genres: Genre[] = [];
  movieForm!: FormGroup;
  isEditing = false;
  loading = true;
  error: string | null = null;
  selectedMovieId: string | null = null;

  constructor(
    private fb: FormBuilder,
    private movieService: MovieService,
    private genreService: GenreService
  ) {}

  ngOnInit(): void {
    this.loadMovies();
    this.loadGenres();

    this.movieForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(2)]],
      description: ['', [Validators.required, Validators.minLength(10)]],
      durationInMinutes: [0, [Validators.required, Validators.min(1)]],
      genreIds: [[], [Validators.required, Validators.minLength(1)]],
      rating: [0, [Validators.required, Validators.min(0), Validators.max(10)]],
      imageUrl: ['', [Validators.required, Validators.pattern('https?://.+')]],
      trailerUrl: ['', [Validators.required, Validators.pattern('https?://.+')]]
    });
  }

  loadMovies(): void {
    this.movieService.getMovies().subscribe(
      (movies) => {
        this.movies = movies.map(movie => ({
          ...movie,
          genreIds: this.getGenreIdsFromNames(movie.genres || [])
        }));
        this.loading = false;
      },
      (error) => {
        this.error = 'Ошибка загрузки фильмов';
        console.error(error);
        this.loading = false;
      }
    );
  }

  loadGenres(): void {
    this.genreService.getGenres().subscribe(
      (genres) => {
        this.genres = genres;
      },
      (error) => {
        console.error('Ошибка загрузки жанров:', error);
      }
    );
  }

  addMovie(): void {
    if (this.movieForm.invalid) return;
    this.movieService.addMovie(this.movieForm.value as Movie).subscribe(() => {
      this.loadMovies();
      this.movieForm.reset();
    });
  }

  editMovie(movie: Movie): void {
    this.selectedMovieId = movie.id || null;
    this.movieForm.setValue({
      title: movie.title,
      description: movie.description,
      durationInMinutes: movie.durationInMinutes,
      genreIds: movie.genreIds,
      rating: movie.rating,
      imageUrl: movie.imageUrl,
      trailerUrl: movie.trailerUrl
    });
    this.isEditing = true;
  }

  updateMovie(): void {
    if (this.movieForm.invalid || !this.selectedMovieId) return;

    this.movieService.updateMovie({ id: this.selectedMovieId, ...this.movieForm.value }).subscribe(() => {
      this.loadMovies();
      this.isEditing = false;
      this.movieForm.reset();
    });
  }

  deleteMovie(id: string): void {
    this.movieService.deleteMovie(id).subscribe(() => {
      this.loadMovies();
    });
  }

  cancelEdit(): void {
    this.isEditing = false;
    this.selectedMovieId = null;
    this.movieForm.reset();
  }

  getGenreIdsFromNames(genres: string[]): string[] {
    return genres
      .map(name => this.genres.find(g => g.name === name)?.id)
      .filter(id => id !== undefined) as string[];
  }

  getGenreNames(genreIds: string[]): string {
    if (!genreIds || genreIds.length === 0) return 'Не указаны';
    return genreIds.map(id => this.genres.find(g => g.id === id)?.name || 'Неизвестно').join(', ');
  }
}
