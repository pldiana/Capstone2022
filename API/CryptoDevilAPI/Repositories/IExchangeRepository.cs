using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI.Repositories
{
    public interface IExchangeRepository
    {
        Task InsertOneAsync(Exchange exchange);
    }
}
