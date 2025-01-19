using MediatR;
using MovieService.Application.Services;
using MovieService.Application.UseCases.Halls.Commands;
using MovieService.Core.Entities;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Halls.Handlers
{
    public class CreateHallCommandHandler : IRequestHandler<CreateHallCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HallService _hallService;

        public CreateHallCommandHandler(IUnitOfWork unitOfWork, HallService hallService)
        {
            _unitOfWork = unitOfWork;
            _hallService = hallService;
        }

        public async Task<Guid> Handle(CreateHallCommand request, CancellationToken cancellationToken)
        {
            if (request.SeatsPerRow.Count != request.NumberOfRows)
            {
                throw new ArgumentException("The number of rows in SeatsPerRow must match NumberOfRows.");
            }

            string seatLayoutJson = _hallService.GenerateSeatLayoutJson(request.NumberOfRows, request.SeatsPerRow);

            int totalSeats = request.SeatsPerRow.Sum();
            if (request.TotalSeats != totalSeats)
            {
                throw new ArgumentException("TotalSeats must match the sum of all seats in SeatsPerRow.");
            }

            var hall = new Hall(
                name: request.Name,
                totalSeats: request.TotalSeats,
                seatLayoutJson: seatLayoutJson);

            await _unitOfWork.Halls.AddAsync(hall, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return hall.Id;
        }
    }
}