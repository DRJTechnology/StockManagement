using StockManagement.Models.Dto.Profile;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class RoleService(IRoleRepository roleRepository) : IRoleService
    {
        public async Task<int> CreateRoleAsync(ApplicationRole role)
        {
            var roleId = await roleRepository.CreateRoleAsync(role);
            return roleId;
        }

        public async Task<ApplicationRole?> GetByIdAsync(int roleId)
        {
            return await roleRepository.GetByIdAsync(roleId);
        }

        public async Task<ApplicationRole> GetByNameAsync(string normalizedName)
        {
            return await roleRepository.GetByNameAsync(normalizedName);
        }

        public async Task<IList<ApplicationRole>> GetByUserIdAsync(int userId)
        {
            return await roleRepository.GetByUserIdAsync(userId);
        }

        public async Task UpdateRole(ApplicationRole role)
        {
            await roleRepository.UpdateRole(role);
        }
    }
}
