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
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminRepository _adminRepository;

        public AdminController(ILogger<AdminController> logger, IAdminRepository adminRepository)
        {
            _logger = logger;
            _adminRepository = adminRepository;
        }

        [HttpGet]
        //[Authorize]
        [Route("exchange")]
        public async Task<ActionResult> GetAllExchanges()
        {         
            var result = await _adminRepository.GetAllExchangesAsync();
            return Ok(result);
        }

        [HttpGet]
        //[Authorize]
        [Route("exchange/{exchangename}")]
        public async Task<ActionResult> GetExchange(string exchangename)
        {
            var result = await _adminRepository.GetOneExchangeAsync(exchangename);
            return Ok(result);
        }

        [HttpPost]
        //[Authorize]
        [Route("exchange")]
        public async Task<ActionResult> InsertExchange(Exchange exchange)
        {
            await _adminRepository.InsertOneExchangeAsync(exchange);
            return Ok();
        }

        [HttpPut]
        //[Authorize]
        [Route("exchange")]
        public async Task<ActionResult> UpdateExchange(Exchange exchange)
        {
            await _adminRepository.UpdateOneExchangeAsync(exchange);
            return Ok();
        }

        [HttpDelete]
        //[Authorize]
        [Route("exchange")]
        public async Task<ActionResult> DeleteExchange(Exchange exchange)
        {
            await _adminRepository.DeleteOneExchangeAsync(exchange);
            return Ok();
        }
    }
}
