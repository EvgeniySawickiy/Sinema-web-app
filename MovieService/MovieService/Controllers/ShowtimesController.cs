using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieService.Application.DTO.Showtime;
using MovieService.Application.UseCases.Showtimes.Commands;
using MovieService.Application.UseCases.Showtimes.Queries;

namespace MovieService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowtimesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShowtimesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowtimeDto>>> GetAllShowtimes(CancellationToken cancellationToken)
        {
            var showtimes = await _mediator.Send(new GetAllShowtimesQuery(), cancellationToken);
            return Ok(showtimes);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ShowtimeDto>> GetShowtimeById(Guid id, CancellationToken cancellationToken)
        {
            var showtime = await _mediator.Send(new GetShowtimeByIdQuery { Id = id }, cancellationToken);
            return Ok(showtime);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateShowtime([FromBody] CreateShowtimeCommand command, CancellationToken cancellationToken)
        {
            var showtimeId = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetShowtimeById), new { id = showtimeId }, showtimeId);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteShowtime(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteShowtimeCommand { Id = id }, cancellationToken);
            return NoContent();
        }
    }
}
