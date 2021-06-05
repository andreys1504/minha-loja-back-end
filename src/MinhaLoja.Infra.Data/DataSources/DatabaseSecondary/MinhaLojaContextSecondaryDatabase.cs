using MinhaLoja.Core.Settings;
using MongoDB.Driver;

namespace MinhaLoja.Infra.Data.DataSources.DatabaseSecondary
{
    public class MinhaLojaContextSecondaryDatabase
    {
        private readonly IMongoDatabase _database;

        public MinhaLojaContextSecondaryDatabase(GlobalSettings globalSettings)
        {
            var mongoClient = new MongoClient(
                connectionString: globalSettings.DatabaseSecondaryConnectionString);

            _database = mongoClient.GetDatabase("MinhaLoja");
        }

        public IMongoCollection<TDocument> GetCollection<TDocument>(string name)
        {
            return _database.GetCollection<TDocument>(name);
        }
    }
}
