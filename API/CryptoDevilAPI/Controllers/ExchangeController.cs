using CryptoDevilAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
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
    [Route("exchanges")]
    public class ExchangeController : ControllerBase
    {
        private readonly ILogger<ExchangeController> _logger;
        private readonly IExchangeRepository _exchangeRepository;

        public ExchangeController(ILogger<ExchangeController> logger, IExchangeRepository exchangeRepository)
        {
            _logger = logger;
            _exchangeRepository = exchangeRepository;
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult> GetAllExchangesAsync()
        {         
            var result = await _exchangeRepository.GetAllExchangesAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("{exchangeName}")]
        //[Authorize]
        public async Task<ActionResult> GetOneExchangeAsync(string exchangeName)
        {
            var result = await _exchangeRepository.GetOneExchangeAsync(exchangeName);
            return Ok(result);
        }

    }
}
