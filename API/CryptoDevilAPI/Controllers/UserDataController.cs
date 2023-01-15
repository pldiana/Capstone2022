using CoinbasePro;
using CoinbasePro.Network.Authentication;
using CryptoDevilAPI.DataAccess;
using CryptoDevilAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoDevilAPI.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("user")]
    public class UserDataController : ControllerBase
    {
        private readonly ILogger<UserDataController> _logger;
        private readonly IUserDataRepository _userRepository;
        private readonly IUserExchangeRepository _userExchangeRepository;

        public UserDataController(ILogger<UserDataController> logger, IUserDataRepository userRepository, IUserExchangeRepository userExchangeRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _userExchangeRepository = userExchangeRepository;                
        }

        [HttpGet]
        //[Authorize]
        [Route("{userId}/data")]
        public async Task<ActionResult> GetPortfolioDataAsync(string userId)
        {
            var userExchange = await _userExchangeRepository.GetOneAsync(userId);
            if (userExchange == null)
            {
                return NotFound();
            }
            else
            {
                var exchangeInstanceList = userExchange.ExchangeList.Where(x => x.Exchange.Name.ToLower() == "coinbase").ToList();
                List<IUserDataDA> userDataList = new List<IUserDataDA>();

                foreach (var exchangeInstance in exchangeInstanceList)
                {
                    userDataList.Add(new CoinbaseUserDataDA(new CoinbaseProClient(new Authenticator(exchangeInstance.Key, exchangeInstance.Signature, exchangeInstance.Phrase), exchangeInstance.IsSandbox.HasValue? exchangeInstance.IsSandbox.Value: false)));
                }

                _userRepository.UserDataList = userDataList;

                var result = await _userRepository.GetPortfolioDataAsync();
                return Ok(result);
            }
        }
    }
}
