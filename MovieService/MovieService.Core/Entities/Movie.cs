using MovieService.Core.Enums;

namespace MovieService.Core.Entities
{
    public class Movie
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required int DurationInMinutes { get; init; }
        public required Genre Genre { get; init; }
        public decimal Rating { get; init; }
    }
}
