using StockManagement.Models.Dto.Profile;

namespace StockManagement.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<int> CreateRoleAsync(ApplicationRole role);
        Task<ApplicationRole> GetByIdAsync(int id);
        Task<IList<ApplicationRole>> GetByUserIdAsync(int userId);
        Task<ApplicationRole> GetByNameAsync(string normalizedName);
        Task UpdateRole(ApplicationRole role);
    }
}
