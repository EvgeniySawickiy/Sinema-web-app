using MediatR;

namespace MovieService.Application.UseCases.Showtimes.Commands
{
    public class DeleteShowtimeCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
