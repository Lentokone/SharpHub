using MongoDB.Bson;
using MongoDB.Driver;

namespace SharpHub.Models.Services
{
    public static class MongoManipulator
    {
        private static string? DATABASE_ADDRESS;
        private static string? DATABASE_NAME;
        private static MongoServerAddress? address;
        private static MongoClientSettings? settings;
        private static MongoClient? client;
        private static IMongoDatabase? database;
        private static IConfiguration? config;

        public static void Initialize(IConfiguration configuration)
        {
            config = configuration;
            var sections = config.GetSection("ConnectionStrings");

            DATABASE_ADDRESS = sections.GetValue<string>("MongoAddress");
            DATABASE_NAME = sections.GetValue<string>("MongoDataBaseName");
            address = new MongoServerAddress(DATABASE_ADDRESS);
            settings = new MongoClientSettings() { Server = address };
            client = new MongoClient(settings);
            database = client.GetDatabase(DATABASE_NAME);
        }

        public static IMongoDatabase GetDB()
        {
            if (database == null) {
                throw new Exception("Database not initialized. Call Initialize first.");
            }
            else
            {
                return database;
            }
        }
    }
}