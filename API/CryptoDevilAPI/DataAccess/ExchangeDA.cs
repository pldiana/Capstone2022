using Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI.DataAccess
{
    public class ExchangeDA : IExchangeDA
    {
        private readonly IMongoCollection<Exchange> _collection;

        public ExchangeDA (IMongoCollection<Exchange> collection)
        {
            _collection = collection;
        }
        public async Task DeleteOneAsync(Exchange exchange)
        {
            var filterId = Builders<Exchange>.Filter.Eq(x => x.Name, exchange?.Name);
            var findId = await _collection.FindOneAndDeleteAsync(filterId);
            if (findId == null)
            { 
                throw new Exception("Record not found.");
            }
        }

        public async Task InsertOneAsync(Exchange exchange)
        {
            if (string.IsNullOrWhiteSpace(exchange?.Name))
            {
                throw new Exception("Exchange requires a Name.");
            }
            var filterId = Builders<Exchange>.Filter.Eq(x => x.Name, exchange?.Name);
            var findId = await _collection.FindAsync(filterId);
            if (findId.ToList().Count > 0)
            {
                throw new Exception("Exchange already exists.");
            }
            await _collection.InsertOneAsync(exchange);
        }

        public async Task UpdateOneAsync(Exchange exchange)
        {
            if (string.IsNullOrWhiteSpace(exchange?.Name))
            {
                throw new Exception("Exchange requires a Name.");
            }
            var filterId = Builders<Exchange>.Filter.Eq(x => x.Name, exchange?.Name);
            var findId = (await _collection.FindAsync(filterId)).SingleOrDefault();
            if (findId != null)
            {
                exchange.Id = findId.Id;
                var replacement = await _collection.ReplaceOneAsync(filterId, exchange);
            }
            else
            {
                throw new Exception("Record not found.");
            }            
        }

        public async Task<List<Exchange>> GetAllExchangesAsync()
        {
            return await _collection.AsQueryable().ToListAsync();
        }
    }
}
