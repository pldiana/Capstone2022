using CryptoDevilAPI.DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI.Repositories
{
    public class ExchangeRepository : IExchangeRepository
    {
        private readonly IExchangeDA _exchangeDA;
        public ExchangeRepository(IExchangeDA exchangeDA)
        {
            _exchangeDA = exchangeDA;
        }
        public async Task<List<Exchange>> GetAllExchangesAsync()
        {
            return await _exchangeDA.GetAllExchangesAsync();
        }

        public async Task<Exchange> GetOneExchangeAsync(string exchangeName)
        {
            return await _exchangeDA.GetExchangeByNameAsync(exchangeName);
        }
    }
}
