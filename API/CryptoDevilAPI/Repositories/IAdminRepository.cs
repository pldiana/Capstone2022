using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI.Repositories
{
    public interface IAdminRepository
    {
        Task InsertOneExchangeAsync(Exchange exchange);
        Task<List<Exchange>> GetAllExchangesAsync();
        Task UpdateOneExchangeAsync(Exchange exchange);
        Task DeleteOneExchangeAsync(Exchange exchange);
        Task<Exchange> GetOneExchangeAsync(string exchangeName);
    }
}
