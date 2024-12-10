namespace BookingService.DataAccess.Persistence
{
    public class MongoDbSettings
    {
        public MongoDbSettings(string connectionString, string databaseName)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
        }

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
