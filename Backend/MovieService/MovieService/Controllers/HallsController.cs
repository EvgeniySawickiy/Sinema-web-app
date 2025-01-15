using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieService.Application.DTO.Hall;
using MovieService.Application.UseCases.Halls.Commands;
using MovieService.Application.UseCases.Halls.Queries;

namespace MovieService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HallsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<HallsController> _logger;

        public HallsController(IMediator mediator, ILogger<HallsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HallDto>>> GetAllHalls(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all halls.");
            var halls = await _mediator.Send(new GetAllHallsQuery(), cancellationToken);
            _logger.LogInformation("Fetched {Count} halls.", halls?.Count() ?? 0);
            return Ok(halls);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<HallDto>> GetHallById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching hall with ID: {HallId}", id);
            var hall = await _mediator.Send(new GetHallByIdQuery { Id = id }, cancellationToken);
            _logger.LogInformation("Fetched hall with ID: {HallId}.", id);
            return Ok(hall);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateHall([FromBody] CreateHallCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating a new hall.");
            var hallId = await _mediator.Send(command, cancellationToken);
            _logger.LogInformation("Created new hall with ID: {HallId}.", hallId);
            return CreatedAtAction(nameof(GetHallById), new { id = hallId }, hallId);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateHall(Guid id, [FromBody] UpdateHallCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating hall with ID: {HallId}.", id);
            command.Id = id;
            var updatedHall = await _mediator.Send(command, cancellationToken);
            _logger.LogInformation("Updated hall with ID: {HallId}.", id);
            return Ok(updatedHall);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteHall(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting hall with ID: {HallId}.", id);
            await _mediator.Send(new DeleteHallCommand { Id = id }, cancellationToken);
            _logger.LogInformation("Deleted hall with ID: {HallId}.", id);
            return NoContent();
        }
    }
}
