using CoinbasePro;
using CoinbasePro.Network.Authentication;
using CryptoDevilAPI.DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI.Repositories
{
    public class UserDataRepository : IUserDataRepository
    {
        private readonly IUserExchangeRepository _userExchangeRepository;
        private readonly ICandleDataDA _candleDataDA;

        public UserDataRepository(IUserExchangeRepository userExchangeRepository, ICandleDataDA candleDataDA)
        {
            _userExchangeRepository = userExchangeRepository;
            _candleDataDA = candleDataDA;
        }

        public async Task<List<Candle>> GetCompletedRunsAsync(string userId)
        {
            var result = await _candleDataDA.GetCandleDataAsync();
            return result;
        }

        public async Task<List<OrderResponse>> GetOrdersFilledAsync(string userId)
        {
            List<OrderResponse> orderResponses = null;

            var userExchange = await _userExchangeRepository.GetOneAsync(userId);
            if (userExchange != null)
            {
                var exchangeInstance = userExchange.ExchangeList.Where(x => x.Exchange.Name.ToLower() == "coinbase").SingleOrDefault();
                var coinbaseData = new CoinbaseUserDataDA(new CoinbaseProClient(new Authenticator(exchangeInstance.Key, exchangeInstance.Signature, exchangeInstance.Phrase), exchangeInstance.IsSandbox.HasValue ? exchangeInstance.IsSandbox.Value : false));
                orderResponses = await coinbaseData.FilledOrdersAsync();

            }

            return orderResponses;
        }

        public async Task<List<OrderResponse>> GetOrdersOpenAsync(string userId)
        {
            List<OrderResponse> orderResponses = null;
            
            var userExchange = await _userExchangeRepository.GetOneAsync(userId);
            if (userExchange != null)
            {
                var exchangeInstance = userExchange.ExchangeList.Where(x => x.Exchange.Name.ToLower() == "coinbase").SingleOrDefault();
                var coinbaseData = new CoinbaseUserDataDA(new CoinbaseProClient(new Authenticator(exchangeInstance.Key, exchangeInstance.Signature, exchangeInstance.Phrase), exchangeInstance.IsSandbox.HasValue ? exchangeInstance.IsSandbox.Value : false));
                orderResponses = await coinbaseData.OpenOrdersAsync();

            }

            return orderResponses;
        }

        public async Task<Dictionary<string, object>> GetPortfolioDataAsync(UserExchange userExchange)
        {
            var exchangeInstanceList = userExchange.ExchangeList.Where(x => x.Exchange.Name.ToLower() == "coinbase").ToList();
            List<IUserDataDA> userDataList = new List<IUserDataDA>();

            foreach (var exchangeInstance in exchangeInstanceList)
            {
                userDataList.Add(new CoinbaseUserDataDA(new CoinbaseProClient(new Authenticator(exchangeInstance.Key, exchangeInstance.Signature, exchangeInstance.Phrase), exchangeInstance.IsSandbox.HasValue ? exchangeInstance.IsSandbox.Value : false)));
            }

            Dictionary<string, object> response = new Dictionary<string, object>();
            decimal sum = 0.0m;

            foreach(var u in userDataList)
            {
                sum += await u.TotalPortfolioValueAsync();

            }
            response.Add("PortfolioAmount", sum);

            return response;
            
        }
    }
}