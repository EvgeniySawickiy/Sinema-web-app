namespace MovieService.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMovieRepository Movies { get; }
        IShowtimeRepository Showtimes { get; }
        IHallRepository Halls { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
