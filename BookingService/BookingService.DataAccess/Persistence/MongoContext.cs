using MongoDB.Driver;

namespace BookingService.DataAccess.Persistence
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
