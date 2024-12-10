using BookingService.DataAccess.Persistence.Interfaces;
using MediatR;

namespace BookingService.Application.Features.Bookings.Commands.Handlers
{
    public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, Unit>
    {
        private readonly IBookingRepository _bookingRepository;

        public CancelBookingCommandHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<Unit> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId);

            if (booking == null)
            {
                throw new Exception("Booking not found");
            }

            booking.Status = Core.Enums.BookingStatus.Canceled;

            await _bookingRepository.UpdateAsync(booking.Id, booking);

            return Unit.Value;
        }
    }
}
