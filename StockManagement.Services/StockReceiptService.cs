using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class StockReceiptService(IMapper mapper, IStockReceiptRepository StockReceiptRepository) : IStockReceiptService
    {
        public async Task<int> CreateAsync(int currentUserId, StockReceiptEditModel StockReceipt)
        {
            var dto = mapper.Map<StockReceiptDto>(StockReceipt);
            return await StockReceiptRepository.CreateAsync(currentUserId, dto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int StockReceiptId)
        {
            return await StockReceiptRepository.DeleteAsync(currentUserId, StockReceiptId);
        }

        public async Task<List<StockReceiptResponseModel>> GetAllAsync()
        {
            var StockReceipts = await StockReceiptRepository.GetAllAsync();
            return mapper.Map<List<StockReceiptResponseModel>>(StockReceipts);
        }

        public async Task<StockReceiptResponseModel> GetByIdAsync(int StockReceiptId)
        {
            var StockReceipt = await StockReceiptRepository.GetByIdAsync(StockReceiptId);
            return mapper.Map<StockReceiptResponseModel>(StockReceipt);
        }

        public async Task<bool> UpdateAsync(int currentUserId, StockReceiptEditModel StockReceipt)
        {
            var dto = mapper.Map<StockReceiptDto>(StockReceipt);
            return await StockReceiptRepository.UpdateAsync(currentUserId, dto);
        }
    }
}
