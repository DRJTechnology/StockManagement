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
    public class VenueController(ILogger<VenueController> logger, IdentityUserAccessor UserAccessor, IVenueService venueService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var venues = await venueService.GetAllAsync();
                return this.Ok(venues);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Get");
                return this.BadRequest();
            }
        }

        [HttpPost]
        public async Task<ApiResponse> Post(VenueEditModel venue)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);
                var newVenueId = await venueService.CreateAsync(appUser.Id, venue);

                return new ApiResponse()
                {
                    CreatedId = newVenueId,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Post");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        [HttpPut()]
        public async Task<ApiResponse> Put(VenueEditModel venue)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                if (await venueService.UpdateAsync(appUser.Id, venue))
                {
                    return new ApiResponse()
                    {
                        CreatedId = venue.Id,
                        Success = true,
                    };
                }
                else
                {
                    return new ApiResponse()
                    {
                        Success = false,
                        ErrorMessage = $"VenueController: Failed to update venue. Id: {venue.Id}"
                    };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Put");
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

                var result = await venueService.DeleteAsync(appUser.Id, Id);

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
