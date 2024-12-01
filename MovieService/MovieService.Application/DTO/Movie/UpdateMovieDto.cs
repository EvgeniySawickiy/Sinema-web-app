namespace MovieService.Application.DTO.Movie
{
    public class UpdateMovieDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? DurationInMinutes { get; set; }
        public string? Genre { get; set; }
        public decimal? Rating { get; set; }
    }
}
