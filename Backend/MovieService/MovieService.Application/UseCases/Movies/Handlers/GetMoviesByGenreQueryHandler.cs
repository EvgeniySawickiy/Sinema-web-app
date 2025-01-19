using MediatR;
using MovieService.Application.DTO.Movie;
using MovieService.Application.UseCases.Movies.Queries;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Movies.Handlers
{
    public class GetMoviesByGenreQueryHandler : IRequestHandler<GetMoviesByGenreQuery, IEnumerable<MovieDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMoviesByGenreQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MovieDto>> Handle(GetMoviesByGenreQuery request, CancellationToken cancellationToken)
        {
            var movies = await _unitOfWork.Movies.FindAsync(
                m => m.MovieGenres.Any(mg => mg.GenreId == request.GenreId), cancellationToken);

            return movies.Select(movie => new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                DurationInMinutes = movie.DurationInMinutes,
                Rating = movie.Rating,
                ImageUrl = movie.ImageUrl,
                TrailerUrl = movie.TrailerUrl,
                Genres = movie.MovieGenres.Select(mg => mg.Genre.Name).ToList(),
            });
        }
    }
}
