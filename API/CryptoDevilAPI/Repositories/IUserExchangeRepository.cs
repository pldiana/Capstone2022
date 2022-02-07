using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI.Repositories
{
    public interface IUserExchangeRepository
    {
        Task InsertOneAsync(UserExchange userExchangeInstance);
        Task UpdateOneAsync(UserExchange userExchangeInstance);
        Task DeleteOneAsync(UserExchange userExchangeInstance);
        Task <UserExchange> GetOneAsync(string userId);
    }
}
