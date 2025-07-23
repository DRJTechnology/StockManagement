using StockManagement.Models.Finance;

namespace StockManagement.Client.Interfaces.Finance
{
    public interface ITransactionDataService
    {
        Task<List<TransactionDetailResponseModel>> GetDetailByAccountTypeAsync(int accountTypeId);
    }
}
