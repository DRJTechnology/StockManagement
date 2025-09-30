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
    public class StockOrderController(ILogger<StockOrderController> logger, IdentityUserAccessor userAccessor, IStockOrderService stockOrderService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var StockOrder = await stockOrderService.GetByIdAsync(id);
                return this.Ok(StockOrder);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(StockOrderController)}: GetById");
                return this.BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var StockOrderList = await stockOrderService.GetAllAsync();
                return Ok(StockOrderList);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(StockOrderController)}: Get");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ApiResponse> Post(StockOrderEditModel model)
        {
            try
            {
                var appUser = await userAccessor.GetRequiredUserAsync(HttpContext);
                var newId = await stockOrderService.CreateAsync(appUser.Id, model);

                return new ApiResponse()
                {
                    CreatedId = newId,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(StockOrderController)}: Post");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        [HttpPut]
        public async Task<ApiResponse> Put(StockOrderEditModel model)
        {
            try
            {
                var appUser = await userAccessor.GetRequiredUserAsync(HttpContext);
                if (await stockOrderService.UpdateAsync(appUser.Id, model))
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
                logger.LogError(ex, $"{nameof(StockOrderController)}: Put");
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
                var result = await stockOrderService.DeleteAsync(appUser.Id, id);

                return new ApiResponse()
                {
                    CreatedId = 0,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(StockOrderController)}: Delete");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        [HttpPost("CreateStockOrderPayments")]
        public async Task<ApiResponse> CreateStockOrderPayments(StockOrderPaymentsCreateModel stockOrderDetailPayments)
        {
            try
            {
                var appUser = await userAccessor.GetRequiredUserAsync(HttpContext);
                var success = await stockOrderService.CreateStockOrderPaymentsAsync(appUser.Id, stockOrderDetailPayments);

                return new ApiResponse()
                {
                    Success = success,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(StockOrderController)}: CreateStockOrderPayments");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        [HttpPost("MarkStockReceived")]
        public async Task<ApiResponse> MarkStockReceived([FromBody] int stockOrderId)
        {
            try
            {
                var appUser = await userAccessor.GetRequiredUserAsync(HttpContext);
                var success = await stockOrderService.MarkStockReceivedAsync(appUser.Id, stockOrderId);

                return new ApiResponse()
                {
                    Success = success,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(StockOrderController)}: MarkStockReceived");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

    }
}
