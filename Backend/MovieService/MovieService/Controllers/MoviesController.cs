using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieService.Application.DTO.Movie;
using MovieService.Application.UseCases.Movies.Commands;
using MovieService.Application.UseCases.Movies.Queries;

namespace MovieService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MoviesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetAllMovies(CancellationToken cancellationToken)
        {
            var movies = await _mediator.Send(new GetAllMoviesQuery(), cancellationToken);
            return Ok(movies);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MovieDto>> GetMovieById(Guid id, CancellationToken cancellationToken)
        {
            var movie = await _mediator.Send(new GetMovieByIdQuery { Id = id }, cancellationToken);
            return Ok(movie);
        }

        [HttpGet("by-genre/{genre}")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMoviesByGenre(string genre, CancellationToken cancellationToken)
        {
            var movies = await _mediator.Send(new GetMoviesByGenreQuery { Genre = genre }, cancellationToken);
            return Ok(movies);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateMovie([FromBody] CreateMovieCommand command, CancellationToken cancellationToken)
        {
            var movieId = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetMovieById), new { id = movieId }, movieId);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateMovie(Guid id, [FromBody] UpdateMovieCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteMovie(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteMovieCommand { Id = id }, cancellationToken);
            return NoContent();
        }
    }
}
