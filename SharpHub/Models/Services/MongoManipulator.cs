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
            if (database == null)
            {
                throw new Exception("Database not initialized. Call Initialize first.");
            }
            else
            {
                return database;
            }
        }

        public static T Save<T>(T record) where T : DB_SaveableObject
        {
            try
            {
                var collection = GetDB().GetCollection<T>(typeof(T).Name);

                if (record._id == ObjectId.Empty)
                {
                    collection.InsertOne(record);
                }
                else
                {
                    var filter = Builders<T>.Filter.Eq("_id", record._id);
                    var existingDocument = collection.Find(filter).FirstOrDefault();
                    
                    if (existingDocument != null)
                    {
                        collection.ReplaceOne(filter, record);
                    }
                    else
                    {
                        collection.InsertOne(record);
                    }
                    
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error saving record: {ex.Message}");
            }
            return record;
        }

        public static T Search<T>(T record) where T : DB_SaveableObject
        {
            var MongoTable = GetDB().GetCollection<T>(typeof(T).Name);

            if(record._id != ObjectId.Empty)
            {
                var filterById = Builders<T>.Filter.Eq("_id", record._id);
                return MongoTable.Find(filterById).FirstOrDefault() ?? throw new ArgumentException($"Ei löytynyt dokumenttia ID:llä {record._id} luokasta {typeof(T).Name}.");
            }
            // Etsitään ensimmäinen ei-null-arvoinen ominaisuus dynaamisesti
            var property = typeof(T).GetProperties()
                .FirstOrDefault(p => p.GetValue(record) != null);

            // Jos ei löydy kelvollista ominaisuutta, heitetään poikkeus
            if (property == null)
            {
                throw new ArgumentException($"Ei löytynyt kelvollista ominaisuutta {typeof(T).Name} luokasta hakua varten.");
            }

            // Haetaan ominaisuuden arvo, jota käytetään haussa
            var fieldValue = property.GetValue(record);
            if (fieldValue == null)
            {
                throw new ArgumentException($"Ominaisuus {property.Name} ei voi olla null haussa.");
            }

            // Luodaan suodatin hakua varten, jossa verrataan ominaisuuden nimeä ja arvoa
            var filter = Builders<T>.Filter.Eq<object>(property.Name, fieldValue);

            // Palautetaan ensimmäinen löydetty dokumentti tai null, jos ei löydy
            return MongoTable.Find(filter).FirstOrDefault();
        }

        public static bool RepositoryExists(string owner, string repoName)
        {
            var filter = Builders<Repository>.Filter.And(
                Builders<Repository>.Filter.Eq(r => r.Owner, owner),
                Builders<Repository>.Filter.Eq(r => r.RepositoryName, repoName)
            );
            var MongoTable = GetDB().GetCollection<Repository>("Repository");
            return MongoTable.Find(filter).Any();
        }

        public static List<string> SearchAllRepositoryNames(string userName)
        {
            var MongoTable = GetDB().GetCollection<Repository>("Repository");
            var filter = Builders<Repository>.Filter.Eq(r => r.Owner, userName);
            var repoNames = MongoTable.Find(filter).Project(r => r.RepositoryName).ToList();

            return repoNames;
        }

        public static List<Repository> SearchAllRepositories(string userName)
        {
            var MongoTable = GetDB().GetCollection<Repository>("Repository");
            var filter = Builders<Repository>.Filter.Eq("Owner", userName);
            return MongoTable.Find(filter).ToList();
        }
    }
}