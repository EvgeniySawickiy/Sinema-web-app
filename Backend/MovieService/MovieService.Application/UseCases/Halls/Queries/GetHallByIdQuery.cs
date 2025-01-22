using MediatR;

using MovieService.Application.DTO.Hall;

namespace MovieService.Application.UseCases.Halls.Queries
{
    public class GetHallByIdQuery : IRequest<HallDto>
    {
        public Guid Id { get; set; }
    }
}
