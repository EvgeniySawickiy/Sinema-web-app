using MovieService.Core.Enums;

namespace MovieService.Core.Entities
{
    public class Movie
    {
        public Movie(string title, string description, int durationInMinutes, Genre genre, decimal rating)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            DurationInMinutes = durationInMinutes;
            Genre = genre;
            Rating = rating;
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int DurationInMinutes { get; private set; }
        public Genre Genre { get; private set; }
        public decimal Rating { get; private set; }

        public void UpdateTitle(string title) => Title = title;
        public void UpdateDescription(string description) => Description = description;
        public void UpdateDuration(int duration) => DurationInMinutes = duration;
        public void UpdateGenre(Genre genre) => Genre = genre;
        public void UpdateRating(decimal rating) => Rating = rating;
    }
}
