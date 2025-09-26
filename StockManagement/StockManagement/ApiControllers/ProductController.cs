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
    public class ProductController(ILogger<ProductController> logger, IdentityUserAccessor UserAccessor, IProductService productService) : ControllerBase
    {
        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var products = await productService.GetAllAsync();
                return this.Ok(products);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ProductController)}: Get");
                return this.BadRequest();
            }
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<ApiResponse> Post(ProductEditModel product)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                var newProductId = await productService.CreateAsync(appUser.Id, product);

                return new ApiResponse()
                {
                    CreatedId = newProductId,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ProductController)}: Post");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        // PUT api/<ProductController>/
        [HttpPut()]
        public async Task<ApiResponse> Put(ProductEditModel product)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                if (await productService.UpdateAsync(appUser.Id, product))
                {
                    return new ApiResponse()
                    {
                        CreatedId = product.Id,
                        Success = true,
                    };
                }
                else
                {
                    return new ApiResponse()
                    {
                        Success = false,
                        ErrorMessage = $"ProductController: Failed to update product. Id: {product.Id}"
                    };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ProductController)}: Put");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int Id)
        {
            try
            {
                var appUser = await UserAccessor.GetRequiredUserAsync(HttpContext);

                var result = await productService.DeleteAsync(appUser.Id, Id);

                return new ApiResponse()
                {
                    CreatedId = 0,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ProductController)}: Delete");
                return new ApiResponse()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
