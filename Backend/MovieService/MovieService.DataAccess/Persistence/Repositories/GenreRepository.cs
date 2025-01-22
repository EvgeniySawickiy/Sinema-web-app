using Microsoft.EntityFrameworkCore;
using MovieService.Core.Entities;
using MovieService.DataAccess.Interfaces;

namespace MovieService.DataAccess.Persistence.Repositories;

public class GenreRepository: Repository<Genre>, IGenreRepository
{
    private new readonly DataContext _context;
    public GenreRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Genre?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.Genres
            .FirstOrDefaultAsync(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase), cancellationToken);
    }
}