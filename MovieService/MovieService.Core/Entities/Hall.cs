namespace MovieService.Core.Entities
{
    public class Hall
    {
        public Hall(string name, int totalSeats)
        {
            Id = Guid.NewGuid();
            Name = name;
            TotalSeats = totalSeats;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int TotalSeats { get; private set; }

        public void UpdateName(string updateName) => Name = updateName;
        public void UpdateTotalSeats(int totalSeats) => TotalSeats = totalSeats;
    }
}
