using MediatR;

namespace MovieService.Application.UseCases.Halls.Commands
{
    public class CreateHallCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public int TotalSeats { get; set; }
    }
}
