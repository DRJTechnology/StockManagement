using StockManagement.Models.Finance;

namespace StockManagement.Services.Interfaces.Finance
{
    public interface IAccountTypeService
    {
        Task<List<AccountTypeResponseModel>> GetAllAsync();
    }
}
