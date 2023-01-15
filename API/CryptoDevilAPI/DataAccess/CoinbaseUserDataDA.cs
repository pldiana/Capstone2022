using CoinbasePro;
using CoinbasePro.Network.Authentication;
using Models;
using Models.CryptoWatch;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI.DataAccess
{
    public class CoinbaseUserDataDA : IUserDataDA
    {
        ICoinbaseProClient _client = null;
        public CoinbaseUserDataDA (ICoinbaseProClient client)
        {
            _client = client;
        }
        public async Task<decimal> TotalPortfolioValueAsync()
        {
            var accounts = (await _client.AccountsService.GetAllAccountsAsync()).ToList();
            var accountList = accounts.Where(acct => acct.Balance > 0.0m).ToList();
            decimal total = 0.0m;
            try
            {
                foreach (var a in accountList)
                {
                    if (new string[] { "usdc", "usd", "usdt" }.Contains(a.Currency.ToLower()) == false)
                    {
                        var exchange = "coinbase-pro";
                        string uriPriceTemplate = "https://api.cryptowat.ch/markets/{0}/{1}usd/price";
                        var symbolPrice = OHLCAPI.GetPrice(string.Format(uriPriceTemplate, exchange, a.Currency.ToLower()));

                        total += (decimal)symbolPrice * a.Balance;
                    }
                    else
                    {
                        total += a.Balance;
                    }

                }
            }
            catch
            {
            }

            return total;
        }
    }
}
