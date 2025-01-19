using MediatR;

namespace MovieService.Application.UseCases.Genres.Commands;

public class CreateGenreCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
}