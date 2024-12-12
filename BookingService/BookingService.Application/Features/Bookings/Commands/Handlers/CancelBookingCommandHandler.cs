using BookingService.Core.Entities;
using BookingService.Core.Events;
using BookingService.Core.Interfaces;
using BookingService.DataAccess.Messaging;
using BookingService.DataAccess.Persistence.Interfaces;
using MediatR;

namespace BookingService.Application.Features.Bookings.Commands.Handlers
{
    public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, Unit>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ISeatReservationService _seatReservationService;
        private readonly IEventPublisher _eventPublisher;

        public CancelBookingCommandHandler(IBookingRepository bookingRepository, ISeatReservationService seatReservationService, IEventPublisher eventPublisher)
        {
            _bookingRepository = bookingRepository;
            _seatReservationService = seatReservationService;
            _eventPublisher = eventPublisher;
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

            var bookingCancelledEvent = new BookingCancelledEvent
            {
                BookingId = booking.Id,
                UserId = booking.UserId,
                ShowtimeId = booking.ShowtimeId,
                SeatNumbers = booking.Seats.Select(seat => (seat.Row, seat.Number)).ToList(),
                CancelledAt = DateTime.UtcNow,
                Reason = request.Reason,
            };

            _eventPublisher.Publish(
                        exchange: "BookingExchange",
                        routingKey: "booking.cancelled",
                        message: bookingCancelledEvent);
            return Unit.Value;
        }
    }
}
