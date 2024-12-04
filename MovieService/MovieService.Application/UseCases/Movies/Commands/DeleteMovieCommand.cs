using MediatR;

namespace MovieService.Application.UseCases.Movies.Commands
{
    public class DeleteMovieCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
