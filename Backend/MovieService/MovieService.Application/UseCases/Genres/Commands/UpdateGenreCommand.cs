using MediatR;

namespace MovieService.Application.UseCases.Genres.Commands;

public class UpdateGenreCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}