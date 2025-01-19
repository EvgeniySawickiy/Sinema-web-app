using BookingService.Core.Entities;
using BookingService.Core.Events;
using BookingService.Core.Interfaces;
using BookingService.DataAccess.Messaging;
using BookingService.DataAccess.Persistence.Interfaces;
using MediatR;

namespace BookingService.Application.Features.Bookings.Commands.Handlers
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ISeatReservationService _seatReservationService;
        private readonly IEventPublisher _eventPublisher;

        public CreateBookingCommandHandler(IBookingRepository bookingRepository, ISeatReservationService seatReservationService, IEventPublisher eventPublisher)
        {
            _bookingRepository = bookingRepository;
            _seatReservationService = seatReservationService;
            _eventPublisher = eventPublisher;
        }

        public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            decimal price = await _seatReservationService.ReserveSeatsAsync(request.ShowtimeId, request.Seats);

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                ShowtimeId = request.ShowtimeId,
                Seats = request.Seats,
                TotalAmount = request.TotalAmount,
                BookingTime = DateTime.UtcNow,
                Status = Core.Enums.BookingStatus.Pending,
                PaymentStatus = Core.Enums.PaymentStatus.Pending,
            };

            await _bookingRepository.AddAsync(booking);

            var bookingCreatedEvent = new BookingCreatedEvent
            {
                BookingId = booking.Id,
                UserId = request.UserId,
                ShowtimeId = request.ShowtimeId,
                SeatNumbers = booking.Seats.Select(seat => (seat.Row, seat.Number)).ToList(),
                TotalPrice = price,
                CreatedAt = DateTime.UtcNow,
            };

            _eventPublisher.Publish(
           exchange: "BookingExchange",
           routingKey: "booking.created",
           message: bookingCreatedEvent);

            return booking.Id;
        }
    }
}
