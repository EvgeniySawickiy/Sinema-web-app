using MediatR;
using MovieService.Application.DTO.Genre;

namespace MovieService.Application.UseCases.Genres.Queries;

public class GetGenreByIdQuery : IRequest<GenreDto>
{
    public Guid Id { get; set; }
}