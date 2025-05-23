using StockManagement.Models.Dto.Profile;

namespace StockManagement.Services.Interfaces
{
    public interface IRoleService
    {
        Task<int> CreateRoleAsync(ApplicationRole role);
        Task<ApplicationRole?> GetByIdAsync(int roleId);
        Task<IList<ApplicationRole>> GetByUserIdAsync(int userId);
        Task<ApplicationRole> GetByNameAsync(string normalizedName);
        Task UpdateRole(ApplicationRole role);
    }
}
