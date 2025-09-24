using StockManagement.Models.Finance;

namespace StockManagement.Client.Interfaces.Finance
{
    public interface ITransactionDataService
    {
        Task<int> CreateExpenseIncomeAsync(TransactionDetailEditModel editTransactionDetail);
        Task<List<TransactionDetailResponseModel>> GetDetailByAccountTypeAsync(int accountTypeId);
        Task<bool> UpdateExpenseIncomeAsync(TransactionDetailEditModel editTransactionDetail);
        Task<bool> DeleteByDetailIdAsync(int transactionDetailId);
        Task<TransactionFilteredResponseModel> GetFilteredAsync(TransactionFilterModel transactionFilterModel);
        Task<decimal> GetTotalAmountFilteredAsync(TransactionFilterModel transactionFilterModel);
        Task<List<TransactionDetailResponseModel>> GetDetailByAccountAsync(int accountId);
    }
}
