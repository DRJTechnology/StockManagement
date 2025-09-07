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
    public class DeliveryNoteController(ILogger<DeliveryNoteController> logger, IdentityUserAccessor userAccessor, IDeliveryNoteService deliveryNoteService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var deliveryNote = await deliveryNoteService.GetByIdAsync(id);
                return this.Ok(deliveryNote);
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
                var deliveryNoteList = await deliveryNoteService.GetAllAsync();
                return Ok(deliveryNoteList);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Get");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ApiResponse> Post(DeliveryNoteEditModel model)
        {
            try
            {
                var appUser = await userAccessor.GetRequiredUserAsync(HttpContext);
                var newId = await deliveryNoteService.CreateAsync(appUser.Id, model);

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
        public async Task<ApiResponse> Put(DeliveryNoteEditModel model)
        {
            try
            {
                var appUser = await userAccessor.GetRequiredUserAsync(HttpContext);
                if (await deliveryNoteService.UpdateAsync(appUser.Id, model))
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
                var result = await deliveryNoteService.DeleteAsync(appUser.Id, id);

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
