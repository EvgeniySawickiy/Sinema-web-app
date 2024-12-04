namespace MovieService.Application.DTO.Showtime
{
    public class ShowtimeDto
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public string MovieTitle { get; set; }
        public DateTime StartTime { get; set; }
        public Guid HallId { get; set; }
        public string HallName { get; set; }
    }
}
