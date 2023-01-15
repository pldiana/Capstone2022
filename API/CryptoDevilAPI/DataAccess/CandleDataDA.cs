using Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoDevilAPI.DataAccess
{
    public class CandleDataDA :ICandleDataDA
    {
        private readonly IMongoCollection<Candle> _collection;

        public CandleDataDA(IMongoCollection<Candle> collection)
        {
            _collection = collection;
        }

        public async Task<List<Candle>> GetCandleDataAsync()
        {
            return await _collection.AsQueryable().ToListAsync();
        }

        public async Task InsertOneAsync(Candle candle)
        {
            await _collection.InsertOneAsync(candle);
        }
    }
}
