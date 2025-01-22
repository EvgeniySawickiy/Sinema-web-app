using MovieService.DataAccess.Interfaces;

namespace MovieService.DataAccess.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(DataContext context, IMovieRepository movieRepository, IShowtimeRepository showtimeRepository, IHallRepository hallRepository, IGenreRepository genreRepository)
        {
            _context = context;
            Movies = movieRepository;
            Showtimes = showtimeRepository;
            Halls = hallRepository;
            Genres = genreRepository;
        }

        private readonly DataContext _context;

        public IMovieRepository Movies { get; }
        public IShowtimeRepository Showtimes { get; }
        public IHallRepository Halls { get; }
        public IGenreRepository Genres { get; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
