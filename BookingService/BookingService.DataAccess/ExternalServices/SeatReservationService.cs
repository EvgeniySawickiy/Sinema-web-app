using BookingService.Core.Entities;
using BookingService.Core.Interfaces;
using BookingService.DataAccess.Persistence;
using MongoDB.Driver;

namespace BookingService.DataAccess.ExternalServices
{
    public class SeatReservationService : ISeatReservationService
    {
        private readonly IMongoCollection<Seat> _seatCollection;
        private readonly MovieServiceGrpcClient _movieServiceClient;

        public SeatReservationService(MongoContext context, MovieServiceGrpcClient movieServiceClient)
        {
            _seatCollection = context.GetCollection<Seat>("Seats");
            _movieServiceClient = movieServiceClient;
        }

        public async Task ReserveSeatsAsync(Guid showtimeId, List<Seat> seats)
        {
            var isValidShowtime = await _movieServiceClient.GetShowtimeInfoAsync(showtimeId.ToString());
            if (isValidShowtime == null || !isValidShowtime.IsActive)
            {
                throw new Exception($"Showtime {showtimeId} is invalid or inactive.");
            }

            foreach (var seat in seats)
            {
                var existingSeat = await _seatCollection.Find(s => s.Row == seat.Row && s.Number == seat.Number && s.HallId == seat.HallId).FirstOrDefaultAsync();
                if (existingSeat != null && existingSeat.IsReserved)
                {
                    throw new Exception($"Seat at Row {seat.Row}, Number {seat.Number} is already reserved.");
                }
                else if (existingSeat is not null)
                {
                    var update = Builders<Seat>.Update.Set(s => s.IsReserved, true);
                    await _seatCollection.UpdateOneAsync(s => s.Id == existingSeat.Id, update);
                }
                else
                {
                    seat.IsReserved = true;
                    await _seatCollection.InsertOneAsync(seat);
                }
            }
        }

        public async Task ReleaseSeatsAsync(Guid showtimeId, List<Seat> seats)
        {
            var isValidShowtime = await _movieServiceClient.GetShowtimeInfoAsync(showtimeId.ToString());
            if (isValidShowtime == null || !isValidShowtime.IsActive)
            {
                throw new Exception($"Showtime {showtimeId} is invalid or inactive.");
            }

            foreach (var seat in seats)
            {
                var existingSeat = await _seatCollection.Find(s => s.Row == seat.Row && s.Number == seat.Number && s.IsReserved).FirstOrDefaultAsync();
                if (existingSeat == null)
                {
                    throw new Exception($"Seat at Row {seat.Row}, Number {seat.Number} is not reserved.");
                }

                var update = Builders<Seat>.Update.Set(s => s.IsReserved, false);
                await _seatCollection.UpdateOneAsync(s => s.Id == existingSeat.Id, update);
            }
        }
    }
}
