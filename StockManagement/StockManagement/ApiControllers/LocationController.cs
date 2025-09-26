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
    public class LocationController(ILogger<LocationController> logger, IdentityUserAccessor UserAccessor, ILocationService locationService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var locations = await locationService.GetAllAsync();
                return this.Ok(locations);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(LocationController)}: Get");
                return this.BadRequest();
            }
        }

        [HttpPost]
        public async Task<ApiResponse> Post(LocationEditModel location)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);
                var newLocationId = await locationService.CreateAsync(appUser.Id, location);

                return new ApiResponse()
                {
                    CreatedId = newLocationId,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(LocationController)}: Post");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        [HttpPut()]
        public async Task<ApiResponse> Put(LocationEditModel location)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                if (await locationService.UpdateAsync(appUser.Id, location))
                {
                    return new ApiResponse()
                    {
                        CreatedId = location.Id,
                        Success = true,
                    };
                }
                else
                {
                    return new ApiResponse()
                    {
                        Success = false,
                        ErrorMessage = $"LocationController: Failed to update location. Id: {location.Id}"
                    };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(LocationController)}: Put");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int Id)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                var result = await locationService.DeleteAsync(appUser.Id, Id);

                return new ApiResponse()
                {
                    CreatedId = 0,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(LocationController)}: Delete");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
