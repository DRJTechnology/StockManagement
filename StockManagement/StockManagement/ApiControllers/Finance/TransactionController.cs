using Microsoft.AspNetCore.Mvc;
using StockManagement.Components.Account;
using StockManagement.Models.Finance;
using StockManagement.Models.InternalObjects;
using StockManagement.Services.Interfaces.Finance;

namespace StockManagement.ApiControllers.Finance
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController(ILogger<TransactionController> logger, IdentityUserAccessor UserAccessor, ITransactionService transactionService) : ControllerBase
    {
        [HttpGet("GetTransactionsByAccountType/{accountTypeId}")]
        public async Task<IActionResult> GetTransactionsByAccountType(int accountTypeId)
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

        [HttpGet("GetTransactionsByAccount/{accountId}")]
        public async Task<IActionResult> GetTransactionsByAccount(int accountId)
        {
            try
            {
                var accounts = await transactionService.GetDetailByAccountAsync(accountId);
                return this.Ok(accounts);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetTransactionsByAccount");
                return this.BadRequest();
            }
        }

        [HttpGet("GetFiltered")]
        public async Task<IActionResult> GetFiltered([FromQuery] TransactionFilterModel transactionFilterModel)
        {
            try
            {
                var filteredTransactions = await transactionService.GetFilteredAsync(transactionFilterModel);
                return this.Ok(filteredTransactions);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetFiltered");
                return this.BadRequest();
            }
        }

        [HttpGet("GetTotalAmountFiltered")]
        public async Task<IActionResult> GetTotalAmountFiltered([FromQuery] TransactionFilterModel transactionFilterModel)
        {
            try
            {
                var totalAmount = await transactionService.GetTotalAmountFilteredAsync(transactionFilterModel);
                return this.Ok(totalAmount);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetFiltered");
                return this.BadRequest();
            }
        }

        [HttpPost("CreateExpenseIncome")]
        public async Task<ApiResponse> CreateExpenseIncome(TransactionDetailEditModel transactionDetail)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                var newAccountId = await transactionService.CreateExpenseIncomeAsync(appUser.Id, transactionDetail);

                return new ApiResponse()
                {
                    CreatedId = newAccountId,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"CreateExpenseIncome");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        [HttpPut("UpdateExpenseIncome")]
        public async Task<ApiResponse> UpdateExpenseIncome(TransactionDetailEditModel transactionDetail)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                if (await transactionService.UpdateExpenseIncomeAsync(appUser.Id, transactionDetail))
                {
                    return new ApiResponse()
                    {
                        CreatedId = transactionDetail.Id,
                        Success = true,
                    };
                }
                else
                {
                    return new ApiResponse()
                    {
                        Success = false,
                        ErrorMessage = $"AccountController: Failed to update expense/income. Id: {transactionDetail.Id}"
                    };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UpdateExpense");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        [HttpDelete("DeleteByDetailId/{id}")]
        public async Task<ApiResponse> DeleteByDetailId(int Id)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                var result = await transactionService.DeleteByDetailIdAsync(appUser.Id, Id);

                return new ApiResponse()
                {
                    CreatedId = 0,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Delete");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
