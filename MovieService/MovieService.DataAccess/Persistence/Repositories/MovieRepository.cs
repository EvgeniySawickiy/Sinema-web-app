using MovieService.Core.Entities;
using MovieService.DataAccess.Interfaces;

namespace MovieService.DataAccess.Persistence.Repositories
{
    public class MovieRepository
        : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(DataContext context)
            : base(context)
        {
        }
    }
}
