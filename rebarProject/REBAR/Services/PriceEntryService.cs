using Microsoft.Extensions.Options;
using MongoDB.Driver;
using REBAR.Configuration;
using REBAR.Models;

namespace REBAR.Services
{
    public class PriceEntryService
    {
        private readonly IMongoCollection<PriceEntry> _priceEntries;

        public PriceEntryService(IMongoDatabase database)
        {
            //var client = new MongoClient(mongoSettings.Value.ConnectionString);
            //var database = client.GetDatabase(mongoSettings.Value.DatabaseName);
            _priceEntries = database.GetCollection<PriceEntry>("PriceEntries");
        }

        public List<PriceEntry> GetAll() => _priceEntries.Find(entry => true).ToList();

        public PriceEntry GetBySize(char size)
            => _priceEntries.Find<PriceEntry>(entry => entry.Size == size).FirstOrDefault();
        public PriceEntry GetBySizeAndIsSpecial(char size,bool isSpecial)
            => _priceEntries.Find<PriceEntry>(entry => entry.Size == size&&entry.IsSpecial==isSpecial).FirstOrDefault();

        public PriceEntry Create(PriceEntry priceEntry)
        {
            _priceEntries.InsertOne(priceEntry);
            return priceEntry;
        }

        public void Update(char size, PriceEntry priceEntry)
            => _priceEntries.ReplaceOne(entry => entry.Size == size, priceEntry);

        public void Remove(char size)
            => _priceEntries.DeleteOne(entry => entry.Size == size);
    }
}
