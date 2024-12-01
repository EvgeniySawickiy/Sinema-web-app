using MovieService.Core.Entities;
using MovieService.DataAccess.Interfaces;

namespace MovieService.DataAccess.Persistence.Repositories
{
    public class ShowtimeRepository : Repository<Showtime>, IShowtimeRepository
    {
        public ShowtimeRepository(DataContext context)
            : base(context)
        {
        }
    }
}
