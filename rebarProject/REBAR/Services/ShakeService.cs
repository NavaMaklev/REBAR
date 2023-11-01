using Microsoft.Extensions.Options;
using MongoDB.Driver;
using REBAR.Configuration;
using REBAR.Models;

namespace REBAR.Services
{
    public class ShakeService
    {
        private readonly IMongoCollection<Shake> _shakes;

        public ShakeService(IMongoDatabase database)
        {
            //var client = new MongoClient(mongoSettings.Value.ConnectionString);
            //var database = client.GetDatabase(mongoSettings.Value.DatabaseName);

            _shakes = database.GetCollection<Shake>("Shakes");
        }

        public List<Shake> GetAll() => _shakes.Find(shake => true).ToList();

        public Shake GetByName(string name)
        {
            return _shakes.Find<Shake>(shake => (shake.Name == name)).FirstOrDefault();
        }

        public Shake Create(Shake shake)
        {
            _shakes.InsertOne(shake);
            return shake;
        }

    }
}
