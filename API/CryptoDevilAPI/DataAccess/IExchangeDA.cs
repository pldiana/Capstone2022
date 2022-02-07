using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI.DataAccess
{
    public interface IExchangeDA
    {
        Task InsertOneAsync(Exchange ExchangeInstance);
        Task UpdateOneAsync(Exchange ExchangeInstance);
        Task DeleteOneAsync(Exchange ExchangeInstance);
        Task<List<Exchange>> GetAllExchangesAsync();
    }
}
