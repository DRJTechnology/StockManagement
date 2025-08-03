using Microsoft.AspNetCore.Mvc;
using StockManagement.Components.Account;
using StockManagement.Models.Finance;
using StockManagement.Models.InternalObjects;
using StockManagement.Services;
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

        [HttpPost("CreateExpense")]
        public async Task<ApiResponse> CreateExpense(TransactionDetailEditModel transactionDetail)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                var newAccountId = await transactionService.CreateExpenseAsync(appUser.Id, transactionDetail);

                return new ApiResponse()
                {
                    CreatedId = newAccountId,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"CreateExpense");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        [HttpPut("UpdateExpense")]
        public async Task<ApiResponse> UpdateExpense(TransactionDetailEditModel transactionDetail)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                if (await transactionService.UpdateExpenseAsync(appUser.Id, transactionDetail))
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
                        ErrorMessage = $"AccountController: Failed to update expense. Id: {transactionDetail.Id}"
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
