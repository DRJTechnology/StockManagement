using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Services.Interfaces.Finance;

namespace StockManagement.ApiControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTypeController(ILogger<AccountTypeController> logger, IAccountTypeService accountTypeService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var accountTypes = await accountTypeService.GetAllAsync();
                return this.Ok(accountTypes);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Get");
                return this.BadRequest();
            }
        }
    }
}
