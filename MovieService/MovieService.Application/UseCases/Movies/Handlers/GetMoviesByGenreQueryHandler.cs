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
            var movies = await _unitOfWork.Movies.FindAsync(m => m.Genre.ToString() == request.Genre, cancellationToken);

            return movies.Select(movie => new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                DurationInMinutes = movie.DurationInMinutes,
                Genre = movie.Genre.ToString(),
                Rating = movie.Rating,
            });
        }
    }
}
