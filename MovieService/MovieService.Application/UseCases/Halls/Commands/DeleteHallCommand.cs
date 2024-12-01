using MediatR;

namespace MovieService.Application.UseCases.Halls.Commands
{
    public class DeleteHallCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
