using BookingService.Core.Entities;
using BookingService.DataAccess.Persistence.Interfaces;
using MediatR;

namespace BookingService.Application.Features.Bookings.Queries.Handlers
{
    public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, Booking>
    {
        private readonly IBookingRepository _bookingRepository;

        public GetBookingByIdQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<Booking> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            return await _bookingRepository.GetByIdAsync(request.BookingId);
        }
    }
}
