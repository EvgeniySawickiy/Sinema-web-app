﻿namespace MovieService.Core.Entities;

public class MovieGenre
{
    public Guid MovieId { get; private set; }
    public Movie Movie { get; private set; }

    public Guid GenreId { get; private set; }
    public Genre Genre { get; private set; }

    public MovieGenre(Guid movieId, Guid genreId)
    {
        MovieId = movieId;
        GenreId = genreId;
    }
}