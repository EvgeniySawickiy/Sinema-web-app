using BookingService.Application.DTO;
using BookingService.Core.Entities;
using MediatR;

namespace BookingService.Application.Features.Bookings.Queries
{
    public class GetBookingsByUserIdQuery : IRequest<IEnumerable<BookingResponseDTO>>
    {
        public Guid UserId { get; set; }
    }
}
