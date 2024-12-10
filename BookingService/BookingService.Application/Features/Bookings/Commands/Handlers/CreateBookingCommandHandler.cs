using BookingService.Core.Entities;
using BookingService.Core.Interfaces;
using BookingService.DataAccess.Persistence.Interfaces;
using MediatR;

namespace BookingService.Application.Features.Bookings.Commands.Handlers
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ISeatReservationService _seatReservationService;

        public CreateBookingCommandHandler(IBookingRepository bookingRepository, ISeatReservationService seatReservationService)
        {
            _bookingRepository = bookingRepository;
            _seatReservationService = seatReservationService;
        }

        public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            await _seatReservationService.ReserveSeatsAsync(request.ShowtimeId, request.Seats);

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

            return booking.Id;
        }
    }
}
