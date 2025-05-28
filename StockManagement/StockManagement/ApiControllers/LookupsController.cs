using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Components.Account;
using StockManagement.Models;
using StockManagement.Services.Interfaces;

namespace StockManagement.ApiControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {
        private readonly ILogger<LookupsController> _logger;
        private readonly ILookupsService _lookupsService;

        public LookupsController(ILogger<LookupsController> logger, ILookupsService lookupsService)
        {
            _logger = logger;
            _lookupsService = lookupsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var lookups = await _lookupsService.GetLookupsAsync();
                return Ok(lookups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching lookups");
                return BadRequest();
            }
        }
    }
}
