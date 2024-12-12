using MediatR;
using MovieService.Application.DTO.Showtime;
using MovieService.Application.UseCases.Showtimes.Queries;
using MovieService.DataAccess.Interfaces;

namespace MovieService.Application.UseCases.Showtimes.Handlers
{
    public class GetAllShowtimesQueryHandler : IRequestHandler<GetAllShowtimesQuery, IEnumerable<ShowtimeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllShowtimesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ShowtimeDto>> Handle(GetAllShowtimesQuery request, CancellationToken cancellationToken)
        {
            var showtimes = await _unitOfWork.Showtimes.GetAllAsync(cancellationToken);

            return showtimes.Select(showtime => new ShowtimeDto
            {
                Id = showtime.Id,
                MovieId = showtime.MovieId,
                MovieTitle = showtime.Movie.Title,
                StartTime = showtime.StartTime,
                HallId = showtime.HallId,
                HallName = showtime.Hall.Name,
                TicketPrice = showtime.TicketPrice,
            });
        }
    }
}
