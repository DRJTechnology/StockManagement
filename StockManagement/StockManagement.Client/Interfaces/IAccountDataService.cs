using StockManagement.Models.Finance;

namespace StockManagement.Client.Interfaces
{
    public interface IAccountDataService : IGenericDataService<AccountEditModel, AccountResponseModel>
    {
        Task<IEnumerable<AccountResponseModel>> GetAllAsync(bool includeInactive);

    }
}
