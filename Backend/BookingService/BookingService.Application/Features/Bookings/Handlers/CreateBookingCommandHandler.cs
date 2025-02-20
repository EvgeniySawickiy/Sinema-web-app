using BookingService.Application.Features.Bookings.Commands;
using BookingService.Core.Entities;
using BookingService.Core.Events;
using BookingService.Core.Interfaces;
using BookingService.DataAccess.ExternalServices;
using BookingService.DataAccess.Messaging;
using BookingService.DataAccess.Persistence.Interfaces;
using MediatR;

namespace BookingService.Application.Features.Bookings.Handlers
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ISeatReservationService _seatReservationService;
        private readonly IEventPublisher _eventPublisher;
        private readonly MovieServiceGrpcClient _movieServiceClient;

        public CreateBookingCommandHandler(
            IBookingRepository bookingRepository,
            ISeatReservationService seatReservationService,
            IEventPublisher eventPublisher,
            MovieServiceGrpcClient movieServiceClient)
        {
            _bookingRepository = bookingRepository;
            _seatReservationService = seatReservationService;
            _eventPublisher = eventPublisher;
            _movieServiceClient = movieServiceClient;
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

            var showtime = await _movieServiceClient.GetShowtimeInfoAsync(booking.ShowtimeId.ToString());
            var bookingCreatedEvent = new BookingCreatedEvent
            {
                BookingId = booking.Id,
                UserId = request.UserId,
                ShowtimeId = request.ShowtimeId,
                SeatNumbers = booking.Seats.Select(seat => (seat.Row, seat.Number)).ToList(),
                TotalPrice = price,
                CreatedAt = DateTime.UtcNow,
                ShowtimeDateTime = showtime.StartTime,
                MovieTitle = showtime.MovieTitle,
            };

            _eventPublisher.Publish(
           exchange: "BookingExchange",
           routingKey: "booking.created",
           message: bookingCreatedEvent);

            return booking.Id;
        }
    }
}
