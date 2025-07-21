using StockManagement.Models.Finance;

namespace StockManagement.Services.Interfaces.Finance
{
    public interface IAccountService
    {
        Task<int> CreateAsync(int currentUserId, AccountEditModel account);
        Task<bool> DeleteAsync(int currentUserId, int accountId);
        Task<List<AccountResponseModel>> GetAllAsync(bool includeInactive);
        Task<AccountResponseModel> GetByIdAsync(int accountId);
        Task<bool> UpdateAsync(int currentUserId, AccountEditModel account);
    }
}
