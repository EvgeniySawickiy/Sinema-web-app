namespace MovieService.Core.Entities
{
    public class Hall
    {
        public Hall(string name, int totalSeats, string seatLayoutJson)
        {
            Id = Guid.NewGuid();
            Name = name;
            TotalSeats = totalSeats;
            SeatLayoutJson = seatLayoutJson;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int TotalSeats { get; private set; }
        public string SeatLayoutJson { get; private set; }

        public ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
        public void UpdateName(string updateName) => Name = updateName;
        public void UpdateTotalSeats(int totalSeats) => TotalSeats = totalSeats;
        public void UpdateSeatLayout(string seatLayoutJson) => SeatLayoutJson = seatLayoutJson;
    }
}