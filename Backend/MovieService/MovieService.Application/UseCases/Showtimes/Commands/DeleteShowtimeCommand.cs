using MediatR;

namespace MovieService.Application.UseCases.Showtimes.Commands
{
    public class DeleteShowtimeCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
