using StockManagement.Models.Dto.Finance;

namespace StockManagement.Repositories.Interfaces.Finanace
{
    public interface IAccountRepository
    {
        Task<int> CreateAsync(int currentUserId, AccountDto productDto);
        Task<bool> DeleteAsync(int currentUserId, int productId);
        Task<bool> UpdateAsync(int currentUserId, AccountDto productDto);
        Task<List<AccountDto>> GetAllAsync(bool includeInactive);
        Task<AccountDto> GetByIdAsync(int accountId);
    }
}
