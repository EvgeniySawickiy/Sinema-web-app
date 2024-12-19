using BookingService.Core.Entities;
using BookingService.Core.Exceptions;
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

        public async Task<decimal> ReserveSeatsAsync(Guid showtimeId, List<Seat> seats)
        {
            var isValidShowtime = await _movieServiceClient.GetShowtimeInfoAsync(showtimeId.ToString());
            if (isValidShowtime == null || !isValidShowtime.IsActive)
            {
                throw new InvalidShowtimeException(showtimeId);
            }

            foreach (var seat in seats)
            {
                var existingSeat = await _seatCollection.Find(s => s.Row == seat.Row && s.Number == seat.Number && s.HallId == seat.HallId).FirstOrDefaultAsync();
                if (existingSeat != null)
                {
                    if (existingSeat.IsReserved)
                    {
                        throw new SeatAlreadyReservedException(seat.Row, seat.Number);
                    }


                    var update = Builders<Seat>.Update.Set(s => s.IsReserved, true);
                    var options = new UpdateOptions { IsUpsert = true };
                    await _seatCollection.UpdateOneAsync(s => s.Id == existingSeat.Id, update, options);
                }
            }

            return decimal.Parse(isValidShowtime.Price);
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
