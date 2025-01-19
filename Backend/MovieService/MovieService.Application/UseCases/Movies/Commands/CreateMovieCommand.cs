using MediatR;

namespace MovieService.Application.UseCases.Movies.Commands
{
    public class CreateMovieCommand : IRequest<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int DurationInMinutes { get; set; }
        public List<Guid> GenreIds { get; set; } = new List<Guid>();
        public decimal Rating { get; set; }
        public string ImageUrl { get; set; }
        public string TrailerUrl { get; set; }
    }
}
