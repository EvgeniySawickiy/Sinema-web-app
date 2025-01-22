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
    private readonly ILogger<GenresController> _logger;

    public GenresController(IMediator mediator, ILogger<GenresController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGenres(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение списка всех жанров.");

        var genres = await _mediator.Send(new GetAllGenresQuery(), cancellationToken);

        _logger.LogInformation("Жанры успешно получены.");

        return Ok(genres);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetGenreById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение жанра с ID: {GenreId}.", id);

        var genre = await _mediator.Send(new GetGenreByIdQuery { Id = id }, cancellationToken);

        _logger.LogInformation("Успешно получен жанр с ID: {GenreId}.", id);

        return Ok(genre);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateGenre([FromBody] CreateGenreCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Создание нового жанра с названием: {GenreName}.", command.Name);

        var genreId = await _mediator.Send(command, cancellationToken);

        _logger.LogInformation("Жанр успешно создан с ID: {GenreId}.", genreId);

        return CreatedAtAction(nameof(GetGenreById), new { id = genreId }, new { id = genreId });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateGenre(Guid id, [FromBody] UpdateGenreCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Обновление жанра с ID: {GenreId}.", id);

        await _mediator.Send(command, cancellationToken);

        _logger.LogInformation("Жанр с ID: {GenreId} успешно обновлен.", id);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteGenre(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Удаление жанра с ID: {GenreId}.", id);

        await _mediator.Send(new DeleteGenreCommand { Id = id }, cancellationToken);

        _logger.LogInformation("Жанр с ID: {GenreId} успешно удален.", id);

        return NoContent();
    }
}