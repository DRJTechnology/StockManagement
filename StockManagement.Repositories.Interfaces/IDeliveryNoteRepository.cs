using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface IDeliveryNoteRepository
    {
        Task<int> CreateAsync(int currentUserId, DeliveryNoteDto deliveryNoteDto);
        Task<bool> DeleteAsync(int currentUserId, int deliveryNoteId);
        Task<bool> UpdateAsync(int currentUserId, DeliveryNoteDto deliveryNoteDto);
        Task<List<DeliveryNoteDto>> GetAllAsync();
        Task<DeliveryNoteDto> GetByIdAsync(int deliveryNoteId);
    }
}
