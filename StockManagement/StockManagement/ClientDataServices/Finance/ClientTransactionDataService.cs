using StockManagement.Client.Interfaces.Finance;
using StockManagement.Models.Finance;

namespace StockManagement.ClientDataServices.Finance
{
    public class ClientTransactionDataService : ITransactionDataService
    {
        public Task<List<TransactionDetailResponseModel>> GetDetailByAccountTypeAsync(int accountTypeId)
        {
            throw new NotImplementedException();
        }
    }
}
