namespace MovieService.Application.DTO.Showtime
{
    public class CreateShowtimeDto
    {
        public Guid MovieId { get; set; }
        public DateTime StartTime { get; set; }
        public Guid HallId { get; set; }
    }
}
