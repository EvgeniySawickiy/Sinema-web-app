namespace MovieService.Application.DTO.Showtime
{
    public class UpdateShowtimeDto
    {
        public DateTime? StartTime { get; set; }
        public Guid? HallId { get; set; }
    }
}
