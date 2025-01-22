using MediatR;
using MovieService.Application.DTO.Hall;
using MovieService.Application.UseCases.Halls.Queries;
using MovieService.Core.Entities;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Halls.Handlers
{
    public class GetAllHallsQueryHandler : IRequestHandler<GetAllHallsQuery, IEnumerable<HallDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllHallsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<HallDto>> Handle(GetAllHallsQuery request, CancellationToken cancellationToken)
        {
            var halls = await _unitOfWork.Halls.GetAllAsync(cancellationToken);

            return halls.Select(hall => MapToHallDto(hall));
        }

        private HallDto MapToHallDto(Hall hall)
        {
            return new HallDto
            {
                Id = hall.Id,
                Name = hall.Name,
                TotalSeats = hall.TotalSeats,
                SeatLayoutJson = hall.SeatLayoutJson,
            };
        }
    }
}
