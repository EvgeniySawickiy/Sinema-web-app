using MediatR;
using MovieService.Application.UseCases.Showtimes.Commands;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Showtimes.Handlers
{
    public class DeleteShowtimeCommandHandler : IRequestHandler<DeleteShowtimeCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteShowtimeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteShowtimeCommand request, CancellationToken cancellationToken)
        {
            var showtime = await _unitOfWork.Showtimes.GetByIdAsync(request.Id, cancellationToken);

            if (showtime == null)
            {
                throw new KeyNotFoundException($"Showtime with ID {request.Id} not found.");
            }

            await _unitOfWork.Showtimes.DeleteAsync(showtime, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
