using Microsoft.AspNetCore.Identity;
using StockManagement.Models.Dto.Profile;

namespace StockManagement.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<int> CreateUserAsync(ApplicationUser user);
        Task CreateUserLoginProviderAsync(string loginProvider, string providerKey, string providerDisplayName, int userId);
        Task DeleteUserAsync(int userId);
        Task<ApplicationUser> GetByEmailAsync(string normalizedEmail);
        Task<ApplicationUser> GetByIdAsync(int id);
        Task<ApplicationUser?> GetByLoginProviderAsync(string loginProvider, string providerKey);
        Task<ApplicationUser> GetByUserNameAsync(string normalizedUserName);
        Task<IList<UserLoginInfo>> GetLoginProviderByUserIdAsync(int userId);
        Task<bool> IsUserInRoleAsync(int id, string roleName);
        Task UpdateUserAsync(ApplicationUser user);
    }
}
