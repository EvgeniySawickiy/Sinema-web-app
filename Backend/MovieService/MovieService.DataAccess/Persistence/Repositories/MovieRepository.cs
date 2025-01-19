using Microsoft.EntityFrameworkCore;
using MovieService.Core.Entities;
using MovieService.DataAccess.Interfaces;

namespace MovieService.DataAccess.Persistence.Repositories
{
    public class MovieRepository
        : Repository<Movie>, IMovieRepository
    {
        private new readonly DataContext _context;

        public MovieRepository(DataContext context)
            : base(context)
        {
            _context = context;
        }

        public new async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .ToListAsync(cancellationToken);
        }

        public new async Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre).FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }
    }
}