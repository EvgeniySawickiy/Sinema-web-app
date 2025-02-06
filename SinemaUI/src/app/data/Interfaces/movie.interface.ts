export interface Movie {
  id: string;
  title: string;
  description: string;
  durationInMinutes: number;
  imageUrl: string;
  trailerUrl: string;
  genres: string[];
  rating: number;
}
