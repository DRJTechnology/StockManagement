using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface IDeliveryNoteService
    {
        Task<int> CreateAsync(int currentUserId, DeliveryNoteEditModel deliveryNote);
        Task<bool> DeleteAsync(int currentUserId, int deliveryNoteId);
        Task<List<DeliveryNoteResponseModel>> GetAllAsync();
        Task<DeliveryNoteResponseModel> GetByIdAsync(int deliveryNoteId);
        Task<bool> UpdateAsync(int currentUserId, DeliveryNoteEditModel deliveryNote);
    }
}
