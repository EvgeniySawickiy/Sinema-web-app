using System.Security.Claims;
using BookingService.Application.DTO;
using BookingService.Application.Features.Bookings.Commands;
using BookingService.Application.Features.Bookings.Queries;
using BookingService.Core.Entities;
using BookingService.Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BookingController> _logger;

        public BookingController(IMediator mediator, ILogger<BookingController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating a new booking.");

            var bookingId = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Created a new booking with ID: {BookingId}.", bookingId);

            return CreatedAtAction(nameof(GetBookingById), new { id = bookingId }, new { Id = bookingId });
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBookingById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching booking with ID: {BookingId}.", id);

            var query = new GetBookingByIdQuery { BookingId = id };
            var booking = await _mediator.Send(query, cancellationToken);

            _logger.LogWarning("Booking with ID: {BookingId} not found.", id);

            return Ok(booking);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllBookings([FromQuery] int? pageNumber, [FromQuery] int? pageSize,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all bookings with pageNumber: {PageNumber}, pageSize: {PageSize}.",
                pageNumber, pageSize);
            var query = new GetAllBookingsQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

            var result = await _mediator.Send(query);

            _logger.LogInformation("Fetched all bookings.");

            return Ok(result);
        }

        [Authorize]
        [HttpGet("user{id:guid}")]
        public async Task<IActionResult> GetBookingsByUserId(Guid userId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching bookings for user ID: {UserId}.", userId);

            var query = new GetBookingsByUserIdQuery
            {
                UserId = userId,
            };
            var bookings = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Fetched {Count} bookings for user ID: {UserId}.", bookings.Count(), userId);

            return Ok(bookings);
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> CancelBooking(Guid id, [FromBody] CancelBookingRequestDTO request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Cancelling booking with ID: {BookingId}. Reason: {Reason}.", id, request.Reason);

            var command = new CancelBookingCommand { BookingId = id, Reason = request.Reason };
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Cancelled booking with ID: {BookingId}.", id);

            return NoContent();
        }
    }
}