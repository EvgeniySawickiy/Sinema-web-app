using MediatR;

namespace MovieService.Application.UseCases.Showtimes.Commands
{
    public class CreateShowtimeCommand : IRequest<Guid>
    {
        public Guid MovieId { get; set; }
        public DateTime StartTime { get; set; }
        public Guid HallId { get; set; }
        public decimal TicketPrice { get; set; }
    }
}
