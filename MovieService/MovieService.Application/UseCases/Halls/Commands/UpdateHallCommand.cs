using MediatR;
using MovieService.Application.DTO.Hall;

namespace MovieService.Application.UseCases.Halls.Commands
{
    public class UpdateHallCommand : IRequest<HallDto>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? TotalSeats { get; set; }
    }
}
