using StockManagement.Models;
using StockManagement.Models.Enums;

namespace StockManagement.Services.Interfaces
{
    public interface IContactService
    {
        Task<int> CreateAsync(int currentUserId, ContactEditModel contact);
        Task<bool> DeleteAsync(int currentUserId, int contactId);
        Task<List<ContactResponseModel>> GetAllAsync();
        Task<List<ContactResponseModel>> GetByTypeAsync(ContactTypeEnum contactType);
        Task<bool> UpdateAsync(int currentUserId, ContactEditModel contact);
    }
}
