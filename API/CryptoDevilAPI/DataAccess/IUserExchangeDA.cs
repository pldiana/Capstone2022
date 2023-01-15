using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI.DataAccess
{
    public interface IUserExchangeDA
    {
        Task InsertOneAsync(UserExchange userExchangeInstance);
        Task UpdateOneAsync(UserExchange userExchangeInstance);
        Task DeleteOneAsync(string userId);
        Task <UserExchange> GetOneAsync(string userId);
        Task<List<UserExchange>> GetActiveAsync();
    }
}
