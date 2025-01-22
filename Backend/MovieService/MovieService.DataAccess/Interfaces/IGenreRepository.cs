using MovieService.Core.Entities;

namespace MovieService.DataAccess.Interfaces;

public interface IGenreRepository: IRepository<Genre>
{
    Task<Genre?> GetByNameAsync(string name, CancellationToken cancellationToken);
}