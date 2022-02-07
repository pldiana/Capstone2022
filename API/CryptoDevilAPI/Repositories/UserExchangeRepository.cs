using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI.Repositories
{
    public class UserExchangeRepository : IUserExchangeRepository
    {
        public UserExchangeRepository()
        {

        }

        public Task DeleteOneAsync(UserExchange userExchangeInstance)
        {
            throw new NotImplementedException();
        }

        public Task<UserExchange> GetOneAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task InsertOneAsync(UserExchange userExchangeInstance)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOneAsync(UserExchange userExchangeInstance)
        {
            throw new NotImplementedException();
        }
    }
}
