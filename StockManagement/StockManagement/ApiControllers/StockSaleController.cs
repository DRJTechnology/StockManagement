using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Components.Account;
using StockManagement.Models;
using StockManagement.Models.InternalObjects;
using StockManagement.Services;
using StockManagement.Services.Interfaces;

namespace StockManagement.ApiControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StockSaleController(ILogger<StockSaleController> logger, IdentityUserAccessor userAccessor, IStockSaleService stockSaleService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var stockSale = await stockSaleService.GetByIdAsync(id);
                return this.Ok(stockSale);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetById");
                return this.BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var stockSaleList = await stockSaleService.GetAllAsync();
                return Ok(stockSaleList);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Get");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ApiResponse> Post(StockSaleEditModel model)
        {
            try
            {
                var appUser = await userAccessor.GetRequiredUserAsync(HttpContext);
                var newId = await stockSaleService.CreateAsync(appUser.Id, model);

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
        public async Task<ApiResponse> Put(StockSaleEditModel model)
        {
            try
            {
                var appUser = await userAccessor.GetRequiredUserAsync(HttpContext);
                if (await stockSaleService.UpdateAsync(appUser.Id, model))
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
                var result = await stockSaleService.DeleteAsync(appUser.Id, id);

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
        [HttpPost("ConfirmStockSale")]
        public async Task<ApiResponse> ConfirmStockSale(StockSaleConfirmationModel stockSaleConfirmation)
        {
            try
            {
                var appUser = await userAccessor.GetRequiredUserAsync(HttpContext);
                var success = await stockSaleService.ConfirmStockSaleAsync(appUser.Id, stockSaleConfirmation);

                return new ApiResponse()
                {
                    Success = success,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ConfirmStockSale");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        [HttpPost("ConfirmStockSalePayment")]
        public async Task<ApiResponse> ConfirmStockSalePayment([FromBody] StockSaleConfirmPaymentModel stockSaleConfirmPaymentModel)
        {
            try
            {
                var appUser = await userAccessor.GetRequiredUserAsync(HttpContext);
                var success = await stockSaleService.ConfirmStockSalePaymentAsync(appUser.Id, stockSaleConfirmPaymentModel);

                return new ApiResponse()
                {
                    Success = success,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ConfirmStockSalePayment");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        [HttpPost("ResetStockSale")]
        public async Task<ApiResponse> ResetStockSale([FromBody] int stockSaleId)
        {
            try
            {
                var appUser = await userAccessor.GetRequiredUserAsync(HttpContext);
                var success = await stockSaleService.ResetStockSaleAsync(appUser.Id, stockSaleId);

                return new ApiResponse()
                {
                    Success = success,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ResetStockSale");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }
        
    }
}
