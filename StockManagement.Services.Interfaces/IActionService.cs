using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface IActionService
    {
        Task<int> CreateAsync(int currentUserId, ActionEditModel action);
        Task<bool> DeleteAsync(int currentUserId, int actionId);
        Task<List<ActionResponseModel>> GetAllAsync();
        Task<bool> UpdateAsync(int currentUserId, ActionEditModel action);
    }
}
