using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class ProductProductTypeService(IMapper mapper, IProductProductTypeRepository productProductTypeRepository) : IProductProductTypeService
    {
        public async Task<int> CreateAsync(int currentUserId, ProductProductTypeEditModel productProductType)
        {
            var productProductTypeDto = mapper.Map<ProductProductTypeDto>(productProductType);
            return await productProductTypeRepository.CreateAsync(currentUserId, productProductTypeDto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int productProductTypeId)
        {
            return await productProductTypeRepository.DeleteAsync(currentUserId, productProductTypeId);
        }

        public async Task<List<ProductProductTypeResponseModel>> GetAllAsync()
        {
            var productProductTypes = mapper.Map<List<ProductProductTypeResponseModel>>(await productProductTypeRepository.GetAllAsync());
            return productProductTypes;
        }

        public async Task<bool> UpdateAsync(int currentUserId, ProductProductTypeEditModel productProductType)
        {
            var productProductTypeDto = mapper.Map<ProductProductTypeDto>(productProductType);
            return await productProductTypeRepository.UpdateAsync(currentUserId, productProductTypeDto);
        }
    }
}
