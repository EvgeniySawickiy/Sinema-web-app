using Grpc.Core;
using MovieService.Grpc;
using static MovieService.Grpc.MovieService;

namespace BookingService.DataAccess.ExternalServices
{
    public class MovieServiceGrpcClient
    {
        private readonly MovieServiceClient _client;

        public MovieServiceGrpcClient(MovieServiceClient client)
        {
            _client = client;
        }

        public async Task<GetShowtimeResponse> GetShowtimeInfoAsync(string showtimeId)
        {
            var request = new GetShowtimeRequest { ShowtimeId = showtimeId };
            return await _client.GetShowtimeInfoAsync(request);
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
