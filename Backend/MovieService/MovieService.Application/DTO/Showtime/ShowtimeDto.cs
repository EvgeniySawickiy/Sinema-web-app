namespace MovieService.Application.DTO.Showtime
{
    public class ShowtimeDto
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public string MovieTitle { get; set; }
        public List<string> MovieGenres { get; set; } = new List<string>();
        public DateTime StartTime { get; set; }
        public Guid HallId { get; set; }
        public string HallName { get; set; }
        public decimal TicketPrice { get; set; }
    }
}
