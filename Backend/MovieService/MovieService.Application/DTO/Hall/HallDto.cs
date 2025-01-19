namespace MovieService.Application.DTO.Hall
{
    public class HallDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TotalSeats { get; set; }

        public string SeatLayoutJson { get; set; }

        public List<RowDto>? SeatLayout { get; set; }
    }

    public class RowDto
    {
        public int RowNumber { get; set; }
        public List<int> Seats { get; set; }
    }
}