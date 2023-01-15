using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoDevilAPI.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using MongoDB.Driver;

namespace TradeProcessor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IMongoClient client = new MongoClient("mongodb+srv://CapstoneTest2022:CryptoDevil2022@cluster0.qa0sl.mongodb.net/CryptoDevil?retryWrites=true&w=majority");
                    IMongoDatabase database = client.GetDatabase("CryptoDevil");
                    var userExchangeCollection = database.GetCollection<UserExchange>("UserExchange");
                    var candleCollection = database.GetCollection<Candle>("Candles");
                    UserExchangeDA userExchangeDA = new UserExchangeDA(userExchangeCollection);
                    CandleDataDA candleDataDA = new CandleDataDA(candleCollection);
                    services.AddSingleton<ICandleDataDA>(candleDataDA);
                    services.AddSingleton<IUserExchangeDA>(userExchangeDA);
                    
                    services.AddHostedService<CryptoDevilWorker>();
                });
    }
}
