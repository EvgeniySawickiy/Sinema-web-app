using BookingService.Application.DTO;
using MediatR;

namespace BookingService.Application.Features.Bookings.Queries;

public class GetAllBookedSeatsQuery : IRequest<List<SeatDTO>>
{
    public Guid ShowtimeId { get; set; }
}