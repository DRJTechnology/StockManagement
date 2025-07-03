using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface IDeliveryNoteDetailRepository
    {
        Task<int> CreateAsync(int currentUserId, DeliveryNoteDetailDto deliveryNoteDetailDto);
        Task<bool> DeleteAsync(int currentUserId, int deliveryNoteId);
        Task<bool> UpdateAsync(int currentUserId, DeliveryNoteDetailDto deliveryNoteDetailDto);
    }
}
