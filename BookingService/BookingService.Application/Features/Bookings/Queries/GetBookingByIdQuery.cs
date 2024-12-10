using BookingService.Core.Entities;
using MediatR;

namespace BookingService.Application.Features.Bookings.Queries
{
    public class GetBookingByIdQuery : IRequest<Booking>
    {
        public Guid BookingId { get; set; }
    }
}
