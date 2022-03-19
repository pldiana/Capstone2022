using NUnit.Framework;
using MongoDB.Driver;
using Models;
using System.Collections.Generic;
using CryptoDevilAPI.DataAccess;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace CryptoDevilAPI_Tests
{
    public class UserExchangeDATests
    {

        MongoClient _client;
        IMongoCollection<UserExchange> _userExchangeCollection;
        UserExchange _userExchange;

        [SetUp]
        public void Setup()
        {
            IMongoClient client = new MongoClient("mongodb://localhost:27017/");
            IMongoDatabase database = client.GetDatabase("CryptoDevil");
            _userExchangeCollection = database.GetCollection<UserExchange>("UserExchange");
        }

        [Test]
        public async Task UserExchangeDAInsertOneAsync_Success()
        {
            IUserExchangeDA userExchangeDA = new UserExchangeDA(_userExchangeCollection);
            //_userExchange.UserId = _userExchange.UserId + Guid.NewGuid().ToString();
            await userExchangeDA.InsertOneAsync(_userExchange);

        }

        //NEW
        [Test]
        public async Task UserExchangeDADeleteOneAsync_Success()
        {
            IUserExchangeDA userExchangeDA = new UserExchangeDA(_userExchangeCollection);
            //_userExchange.UserId = _userExchange.UserId + Guid.NewGuid().ToString();
            await userExchangeDA.InsertOneAsync(_userExchange);
            //var userExchangeTest = await userExchangeDA.GetOneAsync(_userExchange.UserId);
            //Assert.AreEqual(_userExchange.UserId, userExchangeTest.UserId);
           // await userExchangeDA.DeleteOneAsync(_userExchange.UserId);

            //userExchangeTest = await userExchangeDA.GetOneAsync(_userExchange.UserId);
            //Assert.IsNull(userExchangeTest);

        }

        [Test]
        public async Task UserExchangeDAUpdateOneAsync_Success()
        {
            //insert, update, get
            IUserExchangeDA userExchangeDA = new UserExchangeDA(_userExchangeCollection);
            //_userExchange.UserId = _userExchange.UserId + Guid.NewGuid().ToString();
            await userExchangeDA.InsertOneAsync(_userExchange);

            var newExchange = new ExchangeInstance()
            {
                AccountName = "TestAccountName" + Guid.NewGuid().ToString(),
                AutoLiquidate = false,
                Exchange = new Exchange()
                {
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
            };

            _userExchange.ExchangeList.Add(newExchange);
            await userExchangeDA.UpdateOneAsync(_userExchange);
            //var updatedUserExchange = await userExchangeDA.GetOneAsync(_userExchange.UserId);
            //Assert.IsTrue(updatedUserExchange.ExchangeList.Count(x => x.AccountName == newExchange.AccountName) > 0);
        }
    }
}