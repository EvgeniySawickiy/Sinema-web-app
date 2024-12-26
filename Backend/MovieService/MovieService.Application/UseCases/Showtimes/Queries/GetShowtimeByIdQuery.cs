using MediatR;
using MovieService.Application.DTO.Showtime;

namespace MovieService.Application.UseCases.Showtimes.Queries
{
    public class GetShowtimeByIdQuery : IRequest<ShowtimeDto>
    {
        public Guid Id { get; set; }
    }
}
