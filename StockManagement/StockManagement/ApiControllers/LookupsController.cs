using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<LookupsController> logger;
        private readonly ILookupsService lookupsService;

        public LookupsController(ILogger<LookupsController> logger, ILookupsService lookupsService)
        {
            this.logger = logger;
            this.lookupsService = lookupsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var lookups = await lookupsService.GetLookupsAsync();
                return Ok(lookups);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(LookupsController)}: Get");
                return BadRequest();
            }
        }
    }
}
