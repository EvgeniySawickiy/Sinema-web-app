using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieService.Application.UseCases.Genres.Commands;
using MovieService.Application.UseCases.Genres.Queries;

namespace MovieService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly IMediator _mediator;

    public GenresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGenres(CancellationToken cancellationToken)
    {
        var genres = await _mediator.Send(new GetAllGenresQuery(), cancellationToken);
        return Ok(genres);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetGenreById(Guid id, CancellationToken cancellationToken)
    {
        var genre = await _mediator.Send(new GetGenreByIdQuery { Id = id }, cancellationToken);

        return Ok(genre);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateGenre([FromBody] CreateGenreCommand command,
        CancellationToken cancellationToken)
    {
        var genreId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetGenreById), new { id = genreId }, new { id = genreId });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateGenre(Guid id, [FromBody] UpdateGenreCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("Genre ID mismatch.");
        }

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteGenre(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteGenreCommand { Id = id }, cancellationToken);
        return NoContent();
    }
}