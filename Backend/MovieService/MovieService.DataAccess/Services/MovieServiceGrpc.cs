using Grpc.Core;
using MovieService.Core.Entities;
using MovieService.DataAccess.Interfaces;
using MovieService.Grpc;
using static MovieService.Grpc.MovieService;

namespace MovieService.DataAccess.Services
{
    public class MovieServiceGrpc : MovieServiceBase
    {
        private readonly IShowtimeRepository _showTimeRepository;

        public MovieServiceGrpc(IShowtimeRepository showTimeRepository)
        {
            _showTimeRepository = showTimeRepository;
        }

        public override async Task<GetShowtimeResponse> GetShowtimeInfo(GetShowtimeRequest request, ServerCallContext context)
        {
            var showtime = await _showTimeRepository.GetByIdAsync(Guid.Parse(request.ShowtimeId), new CancellationToken());

            if (showtime == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Showtime {request.ShowtimeId} not found"));
            }

            return new GetShowtimeResponse
            {
                ShowtimeId = showtime.Id.ToString(),
                MovieTitle = showtime.Movie.Title,
                StartTime = showtime.StartTime.ToString("o"),
                EndTime = showtime.StartTime.AddMinutes(showtime.Movie.DurationInMinutes).ToString("o"),
                IsActive = showtime.StartTime < DateTime.UtcNow ? true : false,
            };
        }
    }
}
