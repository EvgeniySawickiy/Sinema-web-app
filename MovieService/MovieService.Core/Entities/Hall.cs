namespace MovieService.Core.Entities
{
    public class Hall
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public required string Name { get; init; }
        public required int TotalSeats { get; init; }
    }
}
