using MediatR;
using MovieService.Application.DTO.Hall;

namespace MovieService.Application.UseCases.Halls.Queries
{
    public class GetAllHallsQuery : IRequest<IEnumerable<HallDto>>
    {
    }
}
