using MovieService.Application.DTO.Showtime;

namespace MovieService.Application.Interfaces
{
    public interface IShowtimeService
    {
        Task<IEnumerable<ShowtimeDto>> GetAllShowtimesAsync(CancellationToken cancellationToken);
        Task<ShowtimeDto?> GetShowtimeByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Guid> CreateShowtimeAsync(CreateShowtimeDto showtimeDto, CancellationToken cancellationToken);
        Task DeleteShowtimeAsync(Guid id, CancellationToken cancellationToken);
    }
}
