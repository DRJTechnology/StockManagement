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
    public class ContactController(ILogger<ContactController> logger, IdentityUserAccessor UserAccessor, IContactService ContactService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var contacts = await ContactService.GetAllAsync();
                return this.Ok(contacts);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Get");
                return this.BadRequest();
            }
        }

        [HttpPost]
        public async Task<ApiResponse> Post(ContactEditModel contact)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);
                var newContactId = await ContactService.CreateAsync(appUser.Id, contact);

                return new ApiResponse()
                {
                    CreatedId = newContactId,
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
        public async Task<ApiResponse> Put(ContactEditModel contact)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                if (await ContactService.UpdateAsync(appUser.Id, contact))
                {
                    return new ApiResponse()
                    {
                        CreatedId = contact.Id,
                        Success = true,
                    };
                }
                else
                {
                    return new ApiResponse()
                    {
                        Success = false,
                        ErrorMessage = $"SupplierController: Failed to update product type. Id: {contact.Id}"
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

                var result = await ContactService.DeleteAsync(appUser.Id, Id);

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
