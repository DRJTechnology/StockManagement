using StockManagement.Models.Finance;

namespace StockManagement.Client.Interfaces.Finance
{
    public interface IAccountDataService : IGenericDataService<AccountEditModel, AccountResponseModel>
    {
        Task<IEnumerable<AccountResponseModel>> GetAllAsync(bool includeInactive);
        Task<IEnumerable<AccountResponseModel>> GetByTypeAsync(int accountTypeId);
    }
}
