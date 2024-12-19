using BookingService.Application.DTO;
using MediatR;

namespace BookingService.Application.Features.Bookings.Queries
{
    public class GetBookingByIdQuery : IRequest<BookingResponseDTO>
    {
        public Guid BookingId { get; set; }
    }
}
