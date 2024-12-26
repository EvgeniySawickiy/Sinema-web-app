using MediatR;
using MovieService.Application.DTO.Movie;
using MovieService.Application.UseCases.Movies.Queries;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Movies.Handlers
{
    public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, MovieDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMovieByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MovieDto> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(request.Id, cancellationToken);

            if (movie == null)
            {
                throw new KeyNotFoundException($"Movie with ID {request.Id} not found.");
            }

            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                DurationInMinutes = movie.DurationInMinutes,
                Genre = movie.Genre.ToString(),
                Rating = movie.Rating,
            };
        }
    }
}
