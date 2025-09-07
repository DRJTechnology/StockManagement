using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface IDeliveryNoteDetailService
    {
        Task<int> CreateAsync(int currentUserId, DeliveryNoteDetailEditModel deliveryNote);
        Task<bool> DeleteAsync(int currentUserId, int deliveryNoteId);
        Task<bool> UpdateAsync(int currentUserId, DeliveryNoteDetailEditModel deliveryNote);
    }
}
