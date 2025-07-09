using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class StockReceiptDetailService(IMapper mapper, IStockReceiptDetailRepository StockReceiptDetailRepository) : IStockReceiptDetailService
    {
        public async Task<int> CreateAsync(int currentUserId, StockReceiptDetailEditModel StockReceipt)
        {
            var dto = mapper.Map<StockReceiptDetailDto>(StockReceipt);
            return await StockReceiptDetailRepository.CreateAsync(currentUserId, dto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int StockReceiptId)
        {
            return await StockReceiptDetailRepository.DeleteAsync(currentUserId, StockReceiptId);
        }

        public async Task<bool> UpdateAsync(int currentUserId, StockReceiptDetailEditModel StockReceipt)
        {
            var dto = mapper.Map<StockReceiptDetailDto>(StockReceipt);
            return await StockReceiptDetailRepository.UpdateAsync(currentUserId, dto);
        }
    }
}
