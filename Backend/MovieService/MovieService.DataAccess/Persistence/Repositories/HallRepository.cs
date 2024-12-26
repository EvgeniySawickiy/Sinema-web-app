using MovieService.Core.Entities;
using MovieService.DataAccess.Interfaces;

namespace MovieService.DataAccess.Persistence.Repositories
{
    public class HallRepository : Repository<Hall>, IHallRepository
    {
        public HallRepository(DataContext context)
            : base(context)
        {
        }
    }
}
