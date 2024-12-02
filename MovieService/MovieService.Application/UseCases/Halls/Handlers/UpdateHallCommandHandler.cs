using MediatR;
using MovieService.Application.UseCases.Halls.Commands;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Halls.Handlers
{
    public class UpdateHallCommandHandler : IRequestHandler<UpdateHallCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateHallCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateHallCommand request, CancellationToken cancellationToken)
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

            await _unitOfWork.Halls.UpdateAsync(hall, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
