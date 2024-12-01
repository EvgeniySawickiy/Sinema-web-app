using MovieService.Application.DTO.Movie;

namespace MovieService.Application.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetAllMoviesAsync(CancellationToken cancellationToken);
        Task<MovieDto?> GetMovieByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Guid> CreateMovieAsync(CreateMovieDto movieDto, CancellationToken cancellationToken);
        Task UpdateMovieAsync(Guid id, UpdateMovieDto movieDto, CancellationToken cancellationToken);
        Task DeleteMovieAsync(Guid id, CancellationToken cancellationToken);
    }
}
