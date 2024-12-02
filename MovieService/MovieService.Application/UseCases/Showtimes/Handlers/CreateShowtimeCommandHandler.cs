using MediatR;
using MovieService.Application.UseCases.Showtimes.Commands;
using MovieService.Core.Entities;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Showtimes.Handlers
{
    public class CreateShowtimeCommandHandler : IRequestHandler<CreateShowtimeCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateShowtimeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateShowtimeCommand request, CancellationToken cancellationToken)
        {
            var showtime = new Showtime(
                movieId: request.MovieId,
                startTime: request.StartTime,
                hallId: request.HallId);

            await _unitOfWork.Showtimes.AddAsync(showtime, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return showtime.Id;
        }
    }
}
