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
    public class ActivityController(ILogger<ActivityController> logger, IdentityUserAccessor UserAccessor, IActivityService activityService) : ControllerBase
    {
        // GET: api/<ActivityController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var activities = await activityService.GetAllAsync();
                return this.Ok(activities);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ActivityController)}: Get");
                return this.BadRequest();
            }
        }

        [HttpGet("GetFiltered")]
        public async Task<IActionResult> GetFiltered([FromQuery] ActivityFilterModel activityFilterModel)
        {
            try
            {
                var filteredActivity = await activityService.GetFilteredAsync(activityFilterModel);
                return this.Ok(filteredActivity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ActivityController)}: GetFiltered");
                return this.BadRequest();
            }
        }

        // POST api/<ActivityController>
        [HttpPost]
        public async Task<ApiResponse> Post(ActivityEditModel activity)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                var newActivityId = await activityService.CreateAsync(appUser.Id, activity);

                return new ApiResponse()
                {
                    CreatedId = newActivityId,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ActivityController)}: Post");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        // PUT api/<ActivityController>/
        [HttpPut()]
        public async Task<ApiResponse> Put(ActivityEditModel activity)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                if (await activityService.UpdateAsync(appUser.Id, activity))
                {
                    return new ApiResponse()
                    {
                        CreatedId = activity.Id,
                        Success = true,
                    };
                }
                else
                {
                    return new ApiResponse()
                    {
                        Success = false,
                        ErrorMessage = $"ActivityController: Failed to update activity. Id: {activity.Id}"
                    };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ActivityController)}: Put");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        // DELETE api/<ActivityController>/5
        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int Id)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                var result = await activityService.DeleteAsync(appUser.Id, Id);

                return new ApiResponse()
                {
                    CreatedId = 0,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ActivityController)}: Delete");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
