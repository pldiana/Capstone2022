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
    public class UserExchangeController : ControllerBase
    {
        private readonly ILogger<UserExchangeController> _logger;
        private readonly IUserExchangeRepository _repository;

        public UserExchangeController(ILogger<UserExchangeController> logger, IUserExchangeRepository userExchangeRepository)
        {
            _logger = logger;
            _repository = userExchangeRepository;
        }

        [HttpGet]
        //[Authorize]
        [Route("{userId}/exchange")]
        public async Task<ActionResult> GetUserExchangeAsync(string userId)
        {         
            var result = await _repository.GetOneAsync(userId);
            return Ok(result);
        }

        [HttpPost]
        //[Authorize]
        [Route("exchange")]
        public async Task<ActionResult> InsertUserExchangeAsync(UserExchange userExchange)
        {
            await _repository.InsertOneAsync(userExchange);
            return Ok();
        }

        [HttpPut]
        //[Authorize]
        [Route("exchange")]
        public async Task<ActionResult> UpdateUserExchangeAsync(UserExchange userExchange)
        {
            await _repository.UpdateOneAsync(userExchange);
            return Ok();
        }

        //probably not needed
        [HttpDelete]
        [Route("exchange")]
        //[Authorize]
        public async Task<ActionResult> DeleteUserExchangeAsync(UserExchange userExchange)
        {
            await _repository.DeleteOneAsync(userExchange);
            return Ok();
        }
    }
}
