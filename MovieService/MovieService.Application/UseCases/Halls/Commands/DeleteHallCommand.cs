using MediatR;

namespace MovieService.Application.UseCases.Halls.Commands
{
    public class DeleteHallCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
