export interface Movie {
  id: string;
  title: string;
  description: string;
  durationInMinutes: number;
  genres: string[];
  genreIds: string[];
  imageUrl: string;
  trailerUrl: string;
  rating: number;
}
