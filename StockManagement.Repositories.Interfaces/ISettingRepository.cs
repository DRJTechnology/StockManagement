using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface ISettingRepository
    {
        Task<bool> UpdateAsync(int currentUserId, SettingDto settingDto);
        Task<List<SettingDto>> GetAllAsync();
    }
}
