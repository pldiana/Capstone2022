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
    }
}