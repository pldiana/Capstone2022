using CryptoDevilAPI.DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI.Repositories
{
    public class UserExchangeRepository : IUserExchangeRepository
    {
        private readonly IUserExchangeDA _userExchangeDA;
        public UserExchangeRepository(IUserExchangeDA userExchangeDA)
        {
            _userExchangeDA = userExchangeDA;

        }

        public async Task DeleteOneAsync(UserExchange userExchangeInstance)
        {
            await _userExchangeDA.DeleteOneAsync(userExchangeInstance.User.Id);
        }

        public async Task<UserExchange> GetOneAsync(string userId)
        {
           return await _userExchangeDA.GetOneAsync(userId);
        }

        public async Task InsertOneAsync(UserExchange userExchangeInstance)
        {
            await _userExchangeDA.InsertOneAsync(userExchangeInstance);
        }

        public async Task UpdateOneAsync(UserExchange userExchangeInstance)
        {
            await _userExchangeDA.UpdateOneAsync(userExchangeInstance);
        }
    }
}
