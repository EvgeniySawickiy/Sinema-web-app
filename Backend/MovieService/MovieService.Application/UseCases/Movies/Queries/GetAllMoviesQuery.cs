using MediatR;
using MovieService.Application.DTO.Movie;

namespace MovieService.Application.UseCases.Movies.Queries
{
    public class GetAllMoviesQuery : IRequest<IEnumerable<MovieDto>>
    {
    }
}
