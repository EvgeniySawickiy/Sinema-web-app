using BookingService.Core.Interfaces;
using BookingService.DataAccess.Persistence.Interfaces;
using MediatR;

namespace BookingService.Application.Features.Bookings.Commands.Handlers
{
    public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, Unit>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ISeatReservationService _seatReservationService;

        public CancelBookingCommandHandler(IBookingRepository bookingRepository, ISeatReservationService seatReservationService)
        {
            _bookingRepository = bookingRepository;
            _seatReservationService = seatReservationService;
        }

        public async Task<Unit> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId);

            if (booking == null)
            {
                throw new Exception("Booking not found");
            }

            await _seatReservationService.ReleaseSeatsAsync(booking.ShowtimeId, booking.Seats);
            booking.Status = Core.Enums.BookingStatus.Canceled;
            booking.Seats.ForEach(seat => seat.IsReserved = false);
            await _bookingRepository.UpdateAsync(booking.Id, booking);

            return Unit.Value;
        }
    }
}
