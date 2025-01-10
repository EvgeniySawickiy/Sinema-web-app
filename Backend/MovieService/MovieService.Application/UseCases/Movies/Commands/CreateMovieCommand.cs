using MediatR;

namespace MovieService.Application.UseCases.Movies.Commands
{
    public class CreateMovieCommand : IRequest<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int DurationInMinutes { get; set; }
        public string Genre { get; set; }
        public decimal Rating { get; set; }
    }
}
