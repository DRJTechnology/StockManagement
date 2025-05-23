using Microsoft.AspNetCore.Identity;
using StockManagement.Models.Dto.Profile;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task<int> CreateUserAsync(ApplicationUser user)
        {
            var userId = await userRepository.CreateUserAsync(user);
            return userId;
        }

        public async Task CreateUserLoginProviderAsync(string loginProvider, string providerKey, string providerDisplayName, int userId)
        {
            await userRepository.CreateUserLoginProviderAsync(loginProvider, providerKey, providerDisplayName, userId);
        }

        public async Task<ApplicationUser?> GetByEmailAsync(string normalizedEmail)
        {
            var user = await userRepository.GetByEmailAsync(normalizedEmail);
            return user;
        }

        public async Task<ApplicationUser?> GetByIdAsync(int userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            return user;
        }

        public async Task<ApplicationUser?> GetByLoginProviderAsync(string loginProvider, string providerKey)
        {
            return await userRepository.GetByLoginProviderAsync(loginProvider, providerKey);
        }

        public async Task<ApplicationUser> GetByUserNameAsync(string normalizedUserName)
        {
            var user = await userRepository.GetByUserNameAsync(normalizedUserName);
            return user;
        }

        public async Task UpdateUser(ApplicationUser user)
        {
            await userRepository.UpdateUserAsync(user);
        }

        public async Task<IList<UserLoginInfo>> GetLoginProviderByUserIdAsync(int userId)
        {
            return await userRepository.GetLoginProviderByUserIdAsync(userId);
        }

        public async Task<bool> IsUserInRoleAsync(int id, string roleName)
        {
            return await userRepository.IsUserInRoleAsync(id, roleName);
        }

        public async Task DeleteAsync(int userId)
        {
            await userRepository.DeleteUserAsync(userId);
        }
    }
}
