using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI.DataAccess
{
    public interface IUserDataDA
    {
        Task<decimal> TotalPortfolioValueAsync();
        Task<List<OrderResponse>> OpenOrdersAsync();
        Task<List<OrderResponse>> FilledOrdersAsync();
        Task<List<Candle>> CompletedRunsAsync();
    }
}
