using Microsoft.EntityFrameworkCore;
using MovieService.Core.Entities;
using MovieService.DataAccess.Interfaces;

namespace MovieService.DataAccess.Persistence.Repositories
{
    public class ShowtimeRepository : Repository<Showtime>, IShowtimeRepository
    {
        private new readonly DataContext _context;
        public ShowtimeRepository(DataContext context)
            : base(context)
        {
            _context = context;
        }

        public new async Task<IEnumerable<Showtime>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Showtimes
                .Include(s => s.Hall)
                .Include(s => s.Movie)
                .ToListAsync(cancellationToken);
        }
    }
}
