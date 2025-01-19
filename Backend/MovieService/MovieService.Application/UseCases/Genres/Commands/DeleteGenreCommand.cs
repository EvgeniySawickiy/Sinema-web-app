using MediatR;

namespace MovieService.Application.UseCases.Genres.Commands;

public class DeleteGenreCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}