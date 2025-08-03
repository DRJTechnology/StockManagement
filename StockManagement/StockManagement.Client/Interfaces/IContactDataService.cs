using StockManagement.Models;

namespace StockManagement.Client.Interfaces
{
    public interface IContactDataService : IGenericDataService<ContactEditModel, ContactResponseModel>
    {
        Task<IEnumerable<ContactResponseModel>> GetByTypeAsync(int contactTypeId);
    }
}
