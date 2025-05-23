using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class ProductService(IMapper mapper, IProductRepository productRepository) : IProductService
    {
        public async Task<int> CreateAsync(int currentUserId, ProductEditModel product)
        {
            var productDto = mapper.Map<ProductDto>(product);
            return await productRepository.CreateAsync(currentUserId, productDto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int productId)
        {
            return await productRepository.DeleteAsync(currentUserId, productId);
        }

        public async Task<List<ProductResponseModel>> GetAllAsync()
        {
            var products = mapper.Map<List<ProductResponseModel>>(await productRepository.GetAllAsync());
            return products;
        }

        public async Task<bool> UpdateAsync(int currentUserId, ProductEditModel product)
        {
            var productDto = mapper.Map<ProductDto>(product);
            return await productRepository.UpdateAsync(currentUserId, productDto);
        }
    }
}
