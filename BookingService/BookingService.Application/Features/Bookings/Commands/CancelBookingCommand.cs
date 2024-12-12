using MediatR;

namespace BookingService.Application.Features.Bookings.Commands
{
    public class CancelBookingCommand : IRequest<Unit>
    {
        public Guid BookingId { get; set; }
        public string Reason { get; set; }
    }
}
