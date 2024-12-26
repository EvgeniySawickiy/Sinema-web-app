using MediatR;
using MovieService.Application.UseCases.Halls.Commands;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Halls.Handlers
{
    public class DeleteHallCommandHandler : IRequestHandler<DeleteHallCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteHallCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteHallCommand request, CancellationToken cancellationToken)
        {
            var hall = await _unitOfWork.Halls.GetByIdAsync(request.Id, cancellationToken);

            if (hall == null)
            {
                throw new KeyNotFoundException($"Hall with ID {request.Id} not found.");
            }

            await _unitOfWork.Halls.DeleteAsync(hall, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
