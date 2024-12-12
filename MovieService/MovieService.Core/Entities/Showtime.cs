namespace MovieService.Core.Entities
{
    public class Showtime
    {
        public Showtime(Guid movieId, DateTime startTime, Guid hallId, decimal ticketPrice)
        {
            Id = Guid.NewGuid();
            MovieId = movieId;
            StartTime = startTime;
            HallId = hallId;
            TicketPrice = ticketPrice;
        }

        public Guid Id { get; private set; }
        public Guid MovieId { get; private set; }
        public DateTime StartTime { get; private set; }
        public Guid HallId { get; private set; }
        public decimal TicketPrice { get; private set; }

        public Movie Movie { get; set; }
        public Hall Hall { get; set; }

        public void UpdateMovieId(Guid movieId) => MovieId = movieId;
        public void UpdateStartTime(DateTime startTime) => StartTime = startTime;
        public void UpdateHallId(Guid hallId) => HallId = hallId;
    }
}
