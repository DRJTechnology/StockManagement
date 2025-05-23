using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface IActionRepository
    {
        Task<int> CreateAsync(int currentUserId, ActionDto ActionDto);
        Task<bool> DeleteAsync(int currentUserId, int actionId);
        Task<bool> UpdateAsync(int currentUserId, ActionDto actionDto);
        Task<List<ActionDto>> GetAllAsync();
    }
}
