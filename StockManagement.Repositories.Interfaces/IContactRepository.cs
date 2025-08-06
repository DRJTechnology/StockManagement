using StockManagement.Models.Dto;
using StockManagement.Models.Enums;

namespace StockManagement.Repositories.Interfaces
{
    public interface IContactRepository
    {
        Task<int> CreateAsync(int currentUserId, ContactDto contactDto);
        Task<bool> DeleteAsync(int currentUserId, int contactId);
        Task<bool> UpdateAsync(int currentUserId, ContactDto contactDto);
        Task<List<ContactDto>> GetAllAsync();
        Task<List<ContactDto>> GetByTypeAsync(ContactTypeEnum contactType);
    }
}
