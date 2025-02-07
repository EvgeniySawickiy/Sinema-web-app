namespace MovieService.Application.DTO.Movie
{
    public class MovieDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DurationInMinutes { get; set; }
        public string ImageUrl { get; set; }
        public string TrailerUrl { get; set; }

        public List<string> Genres { get; set; } = new List<string>();
        public decimal Rating { get; set; }
    }
}