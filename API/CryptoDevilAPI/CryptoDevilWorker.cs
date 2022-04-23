using CryptoDevilAPI.DataAccess;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TradeProcessor
{
    public class CryptoDevilWorker : BackgroundService
    {
        private readonly ILogger<CryptoDevilWorker> _logger;
        private readonly IUserExchangeDA _userExchangeDA;
        private List<UserExchange> userExchanges;

        public CryptoDevilWorker (ILogger<CryptoDevilWorker> logger, IUserExchangeDA userExchangeDA)
        {
            _logger = logger;
            _userExchangeDA = userExchangeDA;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{DateTime.Now} Starting Worker Processing Run");


                try
                {
                    await Run();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled Exception");
                }
                _logger.LogInformation($"{DateTime.Now} End Worker Processing Run");
                _logger.LogInformation($"{DateTime.Now} Next Run: {DateTime.Now.AddMilliseconds(300000)}");
                await Task.Delay(300000, stoppingToken);

            }
        }
        private async Task Run()
        {
            var exchanges = await _userExchangeDA.GetActiveAsync();

            //userExchanges = exchanges.Where(x => x.ExchangeList.Any(y => y.IsActive == true)).ToList();
        }
    }
}
