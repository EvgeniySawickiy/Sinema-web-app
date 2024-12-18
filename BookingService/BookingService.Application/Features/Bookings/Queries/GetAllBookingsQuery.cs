using BookingService.Application.DTO;
using MediatR;

namespace BookingService.Application.Features.Bookings.Queries
{
    public class GetAllBookingsQuery : IRequest<PaginatedResult<BookingResponseDTO>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
