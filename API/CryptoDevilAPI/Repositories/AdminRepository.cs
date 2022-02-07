using CryptoDevilAPI.DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IExchangeDA _exchangeDA;
        public AdminRepository(IExchangeDA exchangeDA)
        {
            _exchangeDA = exchangeDA;
        }

        public async Task DeleteOneExchangeAsync(Exchange exchange)
        {
           await _exchangeDA.DeleteOneAsync(exchange);
        }

        public async Task<List<Exchange>> GetAllExchangesAsync()
        {
            return await _exchangeDA.GetAllExchangesAsync();
        }

        public async Task InsertOneExchangeAsync(Exchange exchange)
        {
            await _exchangeDA.InsertOneAsync(exchange);
        }

        public async Task UpdateOneExchangeAsync(Exchange exchange)
        {
            await _exchangeDA.UpdateOneAsync(exchange);
        }
    }
}
