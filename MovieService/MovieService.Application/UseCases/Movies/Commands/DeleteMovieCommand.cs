using MediatR;

namespace MovieService.Application.UseCases.Movies.Commands
{
    public class DeleteMovieCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
