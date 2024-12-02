using MediatR;
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

        public HallsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HallDto>>> GetAllHalls(CancellationToken cancellationToken)
        {
            var halls = await _mediator.Send(new GetAllHallsQuery(), cancellationToken);
            return Ok(halls);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<HallDto>> GetHallById(Guid id, CancellationToken cancellationToken)
        {
            var hall = await _mediator.Send(new GetHallByIdQuery { Id = id }, cancellationToken);
            return Ok(hall);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateHall([FromBody] CreateHallCommand command, CancellationToken cancellationToken)
        {
            var hallId = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetHallById), new { id = hallId }, hallId);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateHall(Guid id, [FromBody] UpdateHallCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteHall(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteHallCommand { Id = id }, cancellationToken);
            return NoContent();
        }
    }
}
