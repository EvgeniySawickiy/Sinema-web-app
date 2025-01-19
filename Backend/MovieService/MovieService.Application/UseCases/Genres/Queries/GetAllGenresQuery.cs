using MediatR;
using MovieService.Application.DTO.Genre;

namespace MovieService.Application.UseCases.Genres.Queries;

public class GetAllGenresQuery : IRequest<IEnumerable<GenreDto>>
{
}