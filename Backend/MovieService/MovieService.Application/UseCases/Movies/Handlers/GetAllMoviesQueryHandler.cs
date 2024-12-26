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

            return movies.Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                DurationInMinutes = m.DurationInMinutes,
                Genre = m.Genre.ToString(),
                Rating = m.Rating,
            });
        }
    }
}
