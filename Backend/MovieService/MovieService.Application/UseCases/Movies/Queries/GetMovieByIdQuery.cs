using MediatR;
using MovieService.Application.DTO.Movie;

namespace MovieService.Application.UseCases.Movies.Queries
{
    public class GetMovieByIdQuery : IRequest<MovieDto>
    {
        public Guid Id { get; set; }
    }
}
