using StockManagement.Client.Interfaces.Finance;
using StockManagement.Models.Finance;

namespace StockManagement.ClientDataServices.Finance
{
    public class ClientTransactionDataService : ITransactionDataService
    {
        public Task<int> CreateExpenseAsync(TransactionDetailEditModel editTransactionDetail)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByDetailIdAsync(int transactionDetailId)
        {
            throw new NotImplementedException();
        }

        public Task<List<TransactionDetailResponseModel>> GetDetailByAccountTypeAsync(int accountTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateExpenseAsync(TransactionDetailEditModel editTransactionDetail)
        {
            throw new NotImplementedException();
        }
    }
}
