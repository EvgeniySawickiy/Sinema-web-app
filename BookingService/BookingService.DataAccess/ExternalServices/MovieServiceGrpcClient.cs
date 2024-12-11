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
        private readonly IServiceProvider _serviceProvider;

        public MovieServiceGrpcClient(MovieServiceClient client, IServiceProvider serviceProvider)
        {
            _client = client;
            _serviceProvider = serviceProvider;
        }

        public async Task<GetShowtimeResponse> GetShowtimeInfoAsync(string showtimeId)
        {
            var cacheKey = $"showtime:{showtimeId}";

            using var scope = _serviceProvider.CreateScope();
            var cacheService = scope.ServiceProvider.GetRequiredService<IRedisCacheService>();

            var cachedShowtime = await cacheService.GetCacheAsync<GetShowtimeResponse>(cacheKey);
            if (cachedShowtime != null)
            {
                return cachedShowtime;
            }

            var request = new GetShowtimeRequest { ShowtimeId = showtimeId };
            var response = await _client.GetShowtimeInfoAsync(request);

            await cacheService.SetCacheAsync(cacheKey, response, TimeSpan.FromMinutes(10));

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
