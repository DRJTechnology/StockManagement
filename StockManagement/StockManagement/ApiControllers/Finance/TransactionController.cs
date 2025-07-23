using Microsoft.AspNetCore.Mvc;
using StockManagement.Components.Account;
using StockManagement.Services.Interfaces.Finance;

namespace StockManagement.ApiControllers.Finance
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController(ILogger<TransactionController> logger, IdentityUserAccessor UserAccessor, ITransactionService transactionService) : ControllerBase
    {
        [HttpGet("GetTransactionsByAccountType/{accountTypeId}")]
        public async Task<IActionResult> Get(int accountTypeId)
        {
            try
            {
                var accounts = await transactionService.GetDetailByAccountTypeAsync(accountTypeId);
                return this.Ok(accounts);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetTransactionsByAccountType");
                return this.BadRequest();
            }
        }
    }
}
