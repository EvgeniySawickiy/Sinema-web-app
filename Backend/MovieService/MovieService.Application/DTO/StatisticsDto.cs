namespace MovieService.Application.DTO;

public class StatisticsDto
{
    public int TotalShowtimes { get; set; }
    public int TotalMovies { get; set; }
    public int TotalHalls { get; set; }
    public int TotalBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<BookingByDayDto> BookingsByDay { get; set; } = new();
}