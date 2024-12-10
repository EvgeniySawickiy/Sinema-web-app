using BookingService.Core.Entities;
using BookingService.DataAccess.Persistence.Interfaces;
using MediatR;

namespace BookingService.Application.Features.Bookings.Queries.Handlers
{
    public class GetBookingsByUserIdQueryHandler : IRequestHandler<GetBookingsByUserIdQuery, IEnumerable<Booking>>
    {
        private readonly IBookingRepository _bookingRepository;

        public GetBookingsByUserIdQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<Booking>> Handle(GetBookingsByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _bookingRepository.GetByUserIdAsync(request.UserId);
        }
    }
}
