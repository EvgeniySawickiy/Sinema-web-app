using MovieService.Application.DTO.Hall;

namespace MovieService.Application.Interfaces
{
    public interface IHallService
    {
        Task<IEnumerable<HallDto>> GetAllHallsAsync(CancellationToken cancellationToken);
        Task<HallDto?> GetHallByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Guid> CreateHallAsync(CreateHallDto hallDto, CancellationToken cancellationToken);
        Task UpdateHallAsync(Guid id, UpdateHallDto hallDto, CancellationToken cancellationToken);
        Task DeleteHallAsync(Guid id, CancellationToken cancellationToken);
    }
}
