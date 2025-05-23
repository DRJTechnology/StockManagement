using Microsoft.AspNetCore.Identity;
using StockManagement.Models.Dto.Profile;

namespace StockManagement.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> CreateUserAsync(ApplicationUser user);
        Task CreateUserLoginProviderAsync(string loginProvider, string providerKey, string providerDisplayName, int userId);
        Task DeleteAsync(int userId);
        Task<ApplicationUser?> GetByEmailAsync(string normalizedEmail);
        Task<ApplicationUser?> GetByIdAsync(int userId);
        Task<ApplicationUser?> GetByLoginProviderAsync(string loginProvider, string providerKey);
        Task<ApplicationUser> GetByUserNameAsync(string normalizedUserName);
        Task<IList<UserLoginInfo>> GetLoginProviderByUserIdAsync(int userId);
        Task<bool> IsUserInRoleAsync(int id, string roleName);
        Task UpdateUser(ApplicationUser user);
    }
}
