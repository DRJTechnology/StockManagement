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
    public class SettingController(ILogger<SettingController> logger, IdentityUserAccessor UserAccessor, ISettingService settingService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var settings = await settingService.GetAllAsync();
                return this.Ok(settings);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(SettingController)}: Get");
                return this.BadRequest();
            }
        }

        [HttpPut()]
        public async Task<ApiResponse> Put(SettingEditModel setting)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                if (await settingService.UpdateAsync(appUser.Id, setting))
                {
                    return new ApiResponse()
                    {
                        CreatedId = setting.Id,
                        Success = true,
                    };
                }
                else
                {
                    return new ApiResponse()
                    {
                        Success = false,
                        ErrorMessage = $"SettingController: Failed to update setting. Id: {setting.Id}"
                    };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(SettingController)}: Put");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
