using Models;
using MongoDB.Driver;
using NUnit.Framework;
using System.Collections.Generic;

namespace ModelsTest
{
    public class Tests
    {
        IMongoCollection<UserExchange> _userExchangeCollection;
        UserExchange _userExchange;

        [SetUp]
        public void Setup()
        {
            IMongoClient client = new MongoClient("mongodb://localhost:27017/");
            IMongoDatabase database = client.GetDatabase("CryptoDevil");
            _userExchangeCollection = database.GetCollection<UserExchange>("UserExchange");
            _userExchange = new UserExchange()
            {
                ExchangeList = new List<ExchangeInstance>()
                {
                    new ExchangeInstance()
                    {
                        AccountName = "TestAccountName",
                        AutoLiquidate = false,
                        Exchange = new Exchange(){
                            AvailableStrategies = new List<Strategy>
                            {
                                new Strategy()
                                {
                                    JsonSchema = string.Empty,
                                    IsActive = true,
                                    Name = "TestStrategyName"                                }
                            }
                        },
                        ExchangeLiquidationPercentage = .10m,
                        Key = "TestKey",
                        Phrase = "TestPhrase",
                        Signature = "TestSignature",
                        StrategyList = new List<StrategyInstance>()
                        {
                            new StrategyInstance()
                            {
                                AutoLiquidate =  false,
                                LiquidationPercentage = .10m,
                                Settings = new Dictionary<string, object>(),
                                StrategyDetail = new Strategy()
                                {
                                    JsonSchema = string.Empty,
                                    IsActive = false,
                                    Name = "TestStrategyName"
                                }
                            }
                        }
                    }
                }
            };

        }

        [Test]
        public void Test1()
        {
            _userExchangeCollection.InsertOneAsync(_userExchange).GetAwaiter().GetResult();
        }
    }
}