using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class SupplierService(IMapper mapper, ISupplierRepository SupplierRepository) : ISupplierService
    {
        public async Task<int> CreateAsync(int currentUserId, SupplierEditModel Supplier)
        {
            var SupplierDto = mapper.Map<SupplierDto>(Supplier);
            return await SupplierRepository.CreateAsync(currentUserId, SupplierDto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int SupplierId)
        {
            return await SupplierRepository.DeleteAsync(currentUserId, SupplierId);
        }

        public async Task<List<SupplierResponseModel>> GetAllAsync()
        {
            var Suppliers = mapper.Map<List<SupplierResponseModel>>(await SupplierRepository.GetAllAsync());
            return Suppliers;
        }

        public async Task<bool> UpdateAsync(int currentUserId, SupplierEditModel Supplier)
        {
            var SupplierDto = mapper.Map<SupplierDto>(Supplier);
            return await SupplierRepository.UpdateAsync(currentUserId, SupplierDto);
        }
    }
}
