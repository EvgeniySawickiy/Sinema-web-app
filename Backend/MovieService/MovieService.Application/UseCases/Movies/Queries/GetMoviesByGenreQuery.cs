using MediatR;
using MovieService.Application.DTO.Movie;

namespace MovieService.Application.UseCases.Movies.Queries
{
    public class GetMoviesByGenreQuery : IRequest<IEnumerable<MovieDto>>
    {
        public Guid GenreId { get; set; }
    }
}
