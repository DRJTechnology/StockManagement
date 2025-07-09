using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Components.Account;
using StockManagement.Models;
using StockManagement.Models.InternalObjects;
using StockManagement.Services.Interfaces;

namespace StockManagement.ApiControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StockReceiptDetailController(ILogger<StockReceiptDetailController> logger, IdentityUserAccessor userAccessor, IStockReceiptDetailService StockReceiptDetailService) : ControllerBase
    {
        [HttpPost]
        public async Task<ApiResponse> Post(StockReceiptDetailEditModel model)
        {
            try
            {
                var appUser = await userAccessor.GetRequiredUserAsync(HttpContext);
                var newId = await StockReceiptDetailService.CreateAsync(appUser.Id, model);

                return new ApiResponse()
                {
                    CreatedId = newId,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Post");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        [HttpPut]
        public async Task<ApiResponse> Put(StockReceiptDetailEditModel model)
        {
            try
            {
                var appUser = await userAccessor.GetRequiredUserAsync(HttpContext);
                if (await StockReceiptDetailService.UpdateAsync(appUser.Id, model))
                {
                    return new ApiResponse()
                    {
                        CreatedId = model.Id,
                        Success = true,
                    };
                }
                else
                {
                    return new ApiResponse()
                    {
                        Success = false,
                        ErrorMessage = $"Failed to update delivery note. Id: {model.Id}"
                    };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Put");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            try
            {
                var appUser = await userAccessor.GetRequiredUserAsync(HttpContext);
                var result = await StockReceiptDetailService.DeleteAsync(appUser.Id, id);

                return new ApiResponse()
                {
                    CreatedId = 0,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Delete");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
