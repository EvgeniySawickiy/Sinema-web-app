using BookingService.DataAccess.Cache;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using MovieService.Grpc;
using static MovieService.Grpc.MovieService;

namespace BookingService.DataAccess.ExternalServices
{
    public class MovieServiceGrpcClient
    {
        private readonly MovieServiceClient _client;
        private readonly IRedisCacheService _redisCacheService;

        public MovieServiceGrpcClient(MovieServiceClient client, IRedisCacheService redisCacheService)
        {
            _client = client;
            _redisCacheService = redisCacheService;
        }

        public async Task<GetShowtimeResponse> GetShowtimeInfoAsync(string showtimeId)
        {
            var cacheKey = $"showtime:{showtimeId}";

            var cachedShowtime = await _redisCacheService.GetAsync<GetShowtimeResponse>(cacheKey);
            if (cachedShowtime != null)
            {
                return cachedShowtime;
            }

            var request = new GetShowtimeRequest { ShowtimeId = showtimeId };
            var response = await _client.GetShowtimeInfoAsync(request);

            await _redisCacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(10));

            return response;
        }

        public async Task StreamShowtimeUpdatesAsync()
        {
            using var call = _client.StreamShowtimeUpdates(new ShowtimeUpdatesRequest());
            await foreach (var update in call.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine($"Update: {update.ShowtimeId}, Type: {update.UpdateType}");
            }
        }
    }
}

