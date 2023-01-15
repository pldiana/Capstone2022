using CryptoDevilAPI.DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI.Repositories
{
    public interface IUserDataRepository
    {
        Task<Dictionary<string, object>> GetPortfolioDataAsync(UserExchange userExchange);
        Task<List<OrderResponse>> GetOrdersFilledAsync(string userId);
        Task<List<OrderResponse>> GetOrdersOpenAsync(string userId);
        Task<List<Candle>> GetCompletedRunsAsync(string userId);
        Task<Dictionary<string, object>> GetPortfolioDataAsync();

        List<IUserDataDA> UserDataList { set; }


    }
}