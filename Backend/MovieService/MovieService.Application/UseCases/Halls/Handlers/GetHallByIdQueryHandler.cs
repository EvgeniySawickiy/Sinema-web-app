using MediatR;
using MovieService.Application.DTO.Hall;
using MovieService.Application.UseCases.Halls.Queries;
using MovieService.Core.Entities;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Halls.Handlers
{
    public class GetHallByIdQueryHandler : IRequestHandler<GetHallByIdQuery, HallDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetHallByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<HallDto> Handle(GetHallByIdQuery request, CancellationToken cancellationToken)
        {
            var hall = await _unitOfWork.Halls.GetByIdAsync(request.Id, cancellationToken);

            if (hall == null)
            {
                throw new KeyNotFoundException($"Hall with ID {request.Id} not found.");
            }

            return MapToHallDto(hall);
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