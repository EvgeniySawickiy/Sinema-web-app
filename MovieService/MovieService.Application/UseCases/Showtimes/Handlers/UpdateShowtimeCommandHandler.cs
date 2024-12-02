using MediatR;
using MovieService.Application.UseCases.Showtimes.Commands;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Showtimes.Handlers
{
    internal class UpdateShowtimeCommandHandler : IRequestHandler<UpdateShowtimeCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateShowtimeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateShowtimeCommand request, CancellationToken cancellationToken)
        {
            var showTime = await _unitOfWork.Showtimes.GetByIdAsync(request.Id, cancellationToken);

            if (showTime == null)
            {
                throw new KeyNotFoundException($"Hall with ID {request.Id} not found.");
            }

            if (request.MovieId != null)
            {
                showTime.UpdateMovieId(request.MovieId.Value);
            }

            if (request.HallId != null)
            {
                showTime.UpdateHallId(request.HallId.Value);
            }

            if (request.StartTime.HasValue)
            {
                showTime.UpdateStartTime(request.StartTime.Value);
            }

            await _unitOfWork.Showtimes.UpdateAsync(showTime, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}