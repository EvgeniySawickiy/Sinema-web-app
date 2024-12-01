namespace MovieService.Core.Entities
{
    public class Showtime
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public required Guid MovieId { get; init; }
        public required DateTime StartTime { get; init; }
        public required Guid HallId { get; init; }

        public Movie? Movie { get; set; }
        public Hall? Hall { get; set; }
    }
}
