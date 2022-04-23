using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace CryptoDevilAPI.DataAccess
{
    public class UserExchangeDA : IUserExchangeDA
    {
        private readonly IMongoCollection<UserExchange> _collection;
        public UserExchangeDA (IMongoCollection<UserExchange> collection)
        {
            _collection = collection;
        }

        public async Task DeleteOneAsync(string userId)
        {
            //NEW
            var filterId = Builders<UserExchange>.Filter.Eq(x => x.User.Id, userId);
            var findId = await _collection.DeleteOneAsync(filterId);
            if (findId == null)
            {
                throw new Exception("Record not found.");
            }
        }

        //NEW
        public async Task<UserExchange> GetOneAsync(string userId)
        {
            var filterId = Builders<UserExchange>.Filter.Eq(x => x.User.Id, userId);
            var findId = await _collection.FindAsync(filterId);
            return await findId.SingleOrDefaultAsync();
        }

        public async Task InsertOneAsync(UserExchange userExchangeInstance)
        {
            if (string.IsNullOrWhiteSpace(userExchangeInstance?.User.Id))
            {
                throw new Exception("UserExchange object requires a UserId.");
            }
            var filterId = Builders<UserExchange>.Filter.Eq(x => x.User.Id, userExchangeInstance.User.Id);
            var findId = await _collection.FindAsync(filterId);
            if (findId.ToList().Count > 0)
            {
                throw new Exception("User already exists in the database.");
            }
            await _collection.InsertOneAsync(userExchangeInstance);
        }

        public async Task UpdateOneAsync(UserExchange userExchangeInstance)
        {
            //NEW
            if (string.IsNullOrWhiteSpace(userExchangeInstance?.User.Id))
            {
                throw new Exception("User Exchange requires a UserId.");
            }
            var filterId = Builders<UserExchange>.Filter.Eq(x => x.User.Id, userExchangeInstance?.User.Id);
            var findId = (await _collection.FindAsync(filterId)).SingleOrDefault();
            if (findId != null)
            {
                userExchangeInstance.Id = findId.Id;
                var replacement = await _collection.ReplaceOneAsync(filterId, userExchangeInstance);
            }
            else
            {
                throw new Exception("Record not found.");
            }
        }

        public async Task <List<UserExchange>> GetActiveAsync()
        {
            //var filterId = Builders<UserExchange>.Filter.Eq(x => x.ExchangeList.Where(y => y.IsActive == true).ToList());
            //var findId = await _collection.FindAsync(filterId);
            //return await findId.SingleOrDefaultAsync();

            return await _collection.AsQueryable().ToListAsync();
        }
    }
}
