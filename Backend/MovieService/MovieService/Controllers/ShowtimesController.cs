using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieService.Application.DTO.Showtime;
using MovieService.Application.UseCases.Showtimes.Commands;
using MovieService.Application.UseCases.Showtimes.Queries;

namespace MovieService.Controllers
{
    [ApiController]
    [Route("api/showtimes")]
    public class ShowtimesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ShowtimesController> _logger;

        public ShowtimesController(IMediator mediator, ILogger<ShowtimesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowtimeDto>>> GetAllShowtimes(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all showtimes.");

            var showtimes = await _mediator.Send(new GetAllShowtimesQuery(), cancellationToken);

            _logger.LogInformation("Fetched {Count} showtimes.", showtimes?.Count() ?? 0);

            return Ok(showtimes);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ShowtimeDto>> GetShowtimeById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching showtime with ID: {ShowtimeId}.", id);

            var showtime = await _mediator.Send(new GetShowtimeByIdQuery { Id = id }, cancellationToken);

            _logger.LogInformation("Fetched showtime with ID: {ShowtimeId}.", id);

            return Ok(showtime);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateShowtime([FromBody] CreateShowtimeCommand command,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating a new showtime.");

            var showtimeId = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Created new showtime with ID: {ShowtimeId}.", showtimeId);

            return CreatedAtAction(nameof(GetShowtimeById), new { id = showtimeId }, showtimeId);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteShowtime(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting showtime with ID: {ShowtimeId}.", id);

            await _mediator.Send(new DeleteShowtimeCommand { Id = id }, cancellationToken);

            _logger.LogInformation("Deleted showtime with ID: {ShowtimeId}.", id);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateShowtime(Guid id, [FromBody] UpdateShowtimeCommand command,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating showtime with ID: {ShowtimeId}.", id);
            
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Updated showtime with ID: {ShowtimeId}.", id);

            return NoContent();
        }
    }
}