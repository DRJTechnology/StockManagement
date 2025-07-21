using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Components.Account;
using StockManagement.Models.Finance;
using StockManagement.Models.InternalObjects;
using StockManagement.Services.Interfaces.Finance;

namespace StockManagement.ApiControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(ILogger<AccountController> logger, IdentityUserAccessor UserAccessor, IAccountService accountService) : ControllerBase
    {
        // GET: api/<AccountController>
        [HttpGet("GetAll/{includeInactive}")]
        public async Task<IActionResult> Get(bool includeInactive)
        {
            try
            {
                var accounts = await accountService.GetAllAsync(includeInactive);
                return this.Ok(accounts);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Get");
                return this.BadRequest();
            }
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var account = await accountService.GetByIdAsync(id);
                return this.Ok(account);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"GetById{id}");
                return this.BadRequest();
            }
        }

        // POST api/<AccountController>
        [HttpPost]
        public async Task<ApiResponse> Post(AccountEditModel account)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                var newAccountId = await accountService.CreateAsync(appUser.Id, account);

                return new ApiResponse()
                {
                    CreatedId = newAccountId,
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

        // PUT api/<AccountController>/
        [HttpPut()]
        public async Task<ApiResponse> Put(AccountEditModel account)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                if (await accountService.UpdateAsync(appUser.Id, account))
                {
                    return new ApiResponse()
                    {
                        CreatedId = account.Id,
                        Success = true,
                    };
                }
                else
                {
                    return new ApiResponse()
                    {
                        Success = false,
                        ErrorMessage = $"AccountController: Failed to update account. Id: {account.Id}"
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

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int Id)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                var result = await accountService.DeleteAsync(appUser.Id, Id);

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
