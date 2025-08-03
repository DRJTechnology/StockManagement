using StockManagement.Models.Finance;

namespace StockManagement.Client.Interfaces.Finance
{
    public interface ITransactionDataService
    {
        Task<int> CreateExpenseAsync(TransactionDetailEditModel editTransactionDetail);
        Task<List<TransactionDetailResponseModel>> GetDetailByAccountTypeAsync(int accountTypeId);
        Task<bool> UpdateExpenseAsync(TransactionDetailEditModel editTransactionDetail);
        Task<bool> DeleteByDetailIdAsync(int transactionDetailId);
    }
}
