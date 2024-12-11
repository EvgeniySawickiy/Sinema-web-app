using BookingService.Application.DTO;
using MediatR;

namespace BookingService.Application.Features.Bookings.Queries
{
    public class GetAllBookingsQuery : IRequest<IEnumerable<BookingResponseDTO>>
    {
    }
}
