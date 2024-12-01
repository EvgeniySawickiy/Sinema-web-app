using MediatR;

namespace MovieService.Application.UseCases.Movies.Commands
{
    public class UpdateMovieCommand : IRequest
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? DurationInMinutes { get; set; }
        public string? Genre { get; set; }
        public decimal? Rating { get; set; }
    }
}
