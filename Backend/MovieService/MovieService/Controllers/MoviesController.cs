using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieService.Application.DTO.Movie;
using MovieService.Application.UseCases.Movies.Commands;
using MovieService.Application.UseCases.Movies.Queries;

namespace MovieService.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(IMediator mediator, ILogger<MoviesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetAllMovies(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all movies.");

            var movies = await _mediator.Send(new GetAllMoviesQuery(), cancellationToken);

            _logger.LogInformation("Fetched {Count} movies.", movies?.Count() ?? 0);

            return Ok(movies);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MovieDto>> GetMovieById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching movie with ID: {MovieId}.", id);

            var movie = await _mediator.Send(new GetMovieByIdQuery { Id = id }, cancellationToken);

            _logger.LogInformation("Fetched movie with ID: {MovieId}.", id);

            return Ok(movie);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMoviesByGenre([FromQuery] Guid genreId,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching movies by genre: {Genre}.", genreId);

            var movies = await _mediator.Send(new GetMoviesByGenreQuery { GenreId = genreId }, cancellationToken);

            _logger.LogInformation("Fetched {Count} movies for genre: {Genre}.", movies?.Count() ?? 0, genreId);

            return Ok(movies);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateMovie([FromBody] CreateMovieCommand command,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating a new movie.");

            var movieId = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Created new movie with ID: {MovieId}.", movieId);

            return CreatedAtAction(nameof(GetMovieById), new { id = movieId }, movieId);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateMovie(Guid id, [FromBody] UpdateMovieCommand command,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating movie with ID: {MovieId}.", id);
            command.Id = id;

            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Updated movie with ID: {MovieId}.", id);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteMovie(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting movie with ID: {MovieId}.", id);

            await _mediator.Send(new DeleteMovieCommand { Id = id }, cancellationToken);

            _logger.LogInformation("Deleted movie with ID: {MovieId}.", id);

            return NoContent();
        }
    }
}