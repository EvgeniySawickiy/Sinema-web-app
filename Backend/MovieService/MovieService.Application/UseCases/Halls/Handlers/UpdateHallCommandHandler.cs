using MediatR;
using MovieService.Application.DTO.Hall;
using MovieService.Application.Services;
using MovieService.Application.UseCases.Halls.Commands;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Halls.Handlers
{
    public class UpdateHallCommandHandler : IRequestHandler<UpdateHallCommand, HallDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HallService _hallService;

        public UpdateHallCommandHandler(IUnitOfWork unitOfWork, HallService hallService)
        {
            _unitOfWork = unitOfWork;
            _hallService = hallService;
        }

        public async Task<HallDto> Handle(UpdateHallCommand request, CancellationToken cancellationToken)
        {
            var hall = await _unitOfWork.Halls.GetByIdAsync(request.Id, cancellationToken);

            if (hall == null)
            {
                throw new KeyNotFoundException($"Hall with ID {request.Id} not found.");
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                hall.UpdateName(request.Name);
            }

            if (request.TotalSeats.HasValue)
            {
                hall.UpdateTotalSeats(request.TotalSeats.Value);
            }
            
            if (request.SeatsPerRow != null)
            {
                string seatLayoutJson = _hallService.GenerateSeatLayoutJson(request.SeatsPerRow.Count, request.SeatsPerRow);
                hall.UpdateSeatLayout(seatLayoutJson);
            }

            await _unitOfWork.Halls.UpdateAsync(hall, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

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
