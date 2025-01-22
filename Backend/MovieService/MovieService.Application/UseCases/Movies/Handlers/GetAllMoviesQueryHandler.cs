using MediatR;
using MovieService.Application.DTO.Movie;
using MovieService.Application.UseCases.Movies.Queries;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Movies.Handlers
{
    public class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, IEnumerable<MovieDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllMoviesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MovieDto>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _unitOfWork.Movies.GetAllAsync(cancellationToken);

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
