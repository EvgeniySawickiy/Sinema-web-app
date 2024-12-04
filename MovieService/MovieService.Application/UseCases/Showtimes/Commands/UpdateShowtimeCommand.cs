using MediatR;

namespace MovieService.Application.UseCases.Showtimes.Commands
{
    public class UpdateShowtimeCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid? MovieId { get; set; }
        public DateTime? StartTime { get; set; }
        public Guid? HallId { get; set; }
    }
}
