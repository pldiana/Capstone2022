using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TradeProcessor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
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

        }
    }
}
