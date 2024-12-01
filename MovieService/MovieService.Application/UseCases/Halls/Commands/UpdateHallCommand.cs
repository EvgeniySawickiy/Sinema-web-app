using MediatR;

namespace MovieService.Application.UseCases.Halls.Commands
{
    public class UpdateHallCommand : IRequest
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? TotalSeats { get; set; }
    }
}
