using BookingService.Application.DTO;
using BookingService.Application.Features.Bookings.Commands;
using BookingService.Application.Features.Bookings.Queries;
using BookingService.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command, CancellationToken cancellationToken)
        {
            var bookingId = await _mediator.Send(command, cancellationToken);
            return Ok(new { Id = bookingId });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBookingById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetBookingByIdQuery { BookingId = id };
            var booking = await _mediator.Send(query, cancellationToken);

            return Ok(booking);
        }

        [HttpGet("bookings")]
        public async Task<IActionResult> GetAllBookings([FromQuery] int? pageNumber, [FromQuery] int? pageSize, CancellationToken cancellationToken)
        {
            var query = new GetAllBookingsQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetBookingsByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var query = new GetBookingsByUserIdQuery { UserId = userId };
            var bookings = await _mediator.Send(query, cancellationToken);
            return Ok(bookings);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> CancelBooking(Guid id, [FromBody] CancelBookingRequestDTO request, CancellationToken cancellationToken)
        {
            var command = new CancelBookingCommand { BookingId = id, Reason = request.Reason };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
