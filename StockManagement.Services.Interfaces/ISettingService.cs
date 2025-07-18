using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface ISettingService
    {
        Task<List<SettingResponseModel>> GetAllAsync();
        Task<bool> UpdateAsync(int currentUserId, SettingEditModel setting);
    }
}
