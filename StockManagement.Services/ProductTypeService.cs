using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class ProductTypeService(IMapper mapper, IProductTypeRepository productTypeRepository) : IProductTypeService
    {
        public async Task<int> CreateAsync(int currentUserId, ProductTypeEditModel productType)
        {
            var productTypeDto = mapper.Map<ProductTypeDto>(productType);
            return await productTypeRepository.CreateAsync(currentUserId, productTypeDto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int productTypeId)
        {
            return await productTypeRepository.DeleteAsync(currentUserId, productTypeId);
        }

        public async Task<List<ProductTypeResponseModel>> GetAllAsync()
        {
            var productTypes = mapper.Map<List<ProductTypeResponseModel>>(await productTypeRepository.GetAllAsync());
            return productTypes;
        }

        public async Task<bool> UpdateAsync(int currentUserId, ProductTypeEditModel productType)
        {
            var productTypeDto = mapper.Map<ProductTypeDto>(productType);
            return await productTypeRepository.UpdateAsync(currentUserId, productTypeDto);
        }
    }
}
