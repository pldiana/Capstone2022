using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI.Repositories
{
    public class ExchangeRepository : IExchangeRepository
    {
        public ExchangeRepository()
        {

        }
        public Task InsertOneAsync(Exchange exchange)
        {
            throw new NotImplementedException();
        }
    }
}
