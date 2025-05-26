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
    public class LookupsController(ILogger<LookupsController> logger,
                                    IActionService actionService,
                                    IProductService productService,
                                    IProductTypeService productTypeService,
                                    IVenueService venueService
                                    ) : ControllerBase
    {
        // GET: api/<ActivityController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<LookupsModel> lookups = new List<LookupsModel> ()
                {
                    new LookupsModel {
                    ActionList = await actionService.GetAllAsync(),
                    ProductList = await productService.GetAllAsync(),
                    ProductTypeList = await productTypeService.GetAllAsync(),
                    VenueList = await venueService.GetAllAsync()
                    }
                };
                return this.Ok(lookups);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Get");
                return this.BadRequest();
            }
        }
    }
}
