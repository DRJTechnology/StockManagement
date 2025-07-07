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
    public class SupplierController(ILogger<SupplierController> logger, IdentityUserAccessor UserAccessor, ISupplierService SupplierService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var Suppliers = await SupplierService.GetAllAsync();
                return this.Ok(Suppliers);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Get");
                return this.BadRequest();
            }
        }

        [HttpPost]
        public async Task<ApiResponse> Post(SupplierEditModel Supplier)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);
                var newSupplierId = await SupplierService.CreateAsync(appUser.Id, Supplier);

                return new ApiResponse()
                {
                    CreatedId = newSupplierId,
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
        public async Task<ApiResponse> Put(SupplierEditModel Supplier)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                if (await SupplierService.UpdateAsync(appUser.Id, Supplier))
                {
                    return new ApiResponse()
                    {
                        CreatedId = Supplier.Id,
                        Success = true,
                    };
                }
                else
                {
                    return new ApiResponse()
                    {
                        Success = false,
                        ErrorMessage = $"SupplierController: Failed to update product type. Id: {Supplier.Id}"
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

                var result = await SupplierService.DeleteAsync(appUser.Id, Id);

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
