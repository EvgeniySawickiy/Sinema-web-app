using BookingService.Core.Entities;
using MediatR;

namespace BookingService.Application.Features.Bookings.Commands
{
    public class CreateBookingCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public Guid ShowtimeId { get; set; }
        public List<Seat> Seats { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }
}
