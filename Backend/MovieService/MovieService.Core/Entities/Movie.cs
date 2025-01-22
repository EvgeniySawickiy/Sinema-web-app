namespace MovieService.Core.Entities
{
    public class Movie
    {
        public Movie(string title, string description, int durationInMinutes, decimal rating,
            string imageUrl, string trailerUrl)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            DurationInMinutes = durationInMinutes;
            Rating = rating;
            ImageUrl = imageUrl;
            TrailerUrl = trailerUrl;
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int DurationInMinutes { get; private set; }
        public decimal Rating { get; private set; }
        public string ImageUrl { get; private set; }
        public string TrailerUrl { get; private set; }

        public ICollection<MovieGenre> MovieGenres { get; private set; } = new List<MovieGenre>();

        public void UpdateTitle(string title) => Title = title;
        public void UpdateDescription(string description) => Description = description;
        public void UpdateDuration(int duration) => DurationInMinutes = duration;
        public void UpdateRating(decimal rating) => Rating = rating;
        public void UpdateImage(string imageUrl) => ImageUrl = imageUrl;
        public void UpdateTrailer(string trailerUrl) => TrailerUrl = trailerUrl;
    }
}