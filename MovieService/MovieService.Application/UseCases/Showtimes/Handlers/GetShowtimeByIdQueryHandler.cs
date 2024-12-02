using MediatR;
using MovieService.Application.DTO.Showtime;
using MovieService.Application.UseCases.Showtimes.Queries;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Showtimes.Handlers
{
    public class GetShowtimeByIdQueryHandler : IRequestHandler<GetShowtimeByIdQuery, ShowtimeDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetShowtimeByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ShowtimeDto> Handle(GetShowtimeByIdQuery request, CancellationToken cancellationToken)
        {
            var showtime = await _unitOfWork.Showtimes.GetByIdAsync(request.Id, cancellationToken);

            if (showtime == null)
            {
                throw new KeyNotFoundException($"Showtime with ID {request.Id} not found.");
            }

            return new ShowtimeDto
            {
                Id = showtime.Id,
                MovieId = showtime.MovieId,
                MovieTitle = showtime.Movie.Title,
                StartTime = showtime.StartTime,
                HallId = showtime.HallId,
                HallName = showtime.Hall.Name,
            };
        }
    }
}
