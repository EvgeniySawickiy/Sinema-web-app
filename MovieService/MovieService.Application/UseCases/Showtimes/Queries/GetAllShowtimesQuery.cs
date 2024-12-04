using MediatR;
using MovieService.Application.DTO.Showtime;

namespace MovieService.Application.UseCases.Showtimes.Queries
{
    public class GetAllShowtimesQuery : IRequest<IEnumerable<ShowtimeDto>>
    {
    }
}
