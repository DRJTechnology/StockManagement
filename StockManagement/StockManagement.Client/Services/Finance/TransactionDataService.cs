using StockManagement.Client.Interfaces.Finance;
using StockManagement.Models.Finance;
using System.Net.Http.Json;

namespace StockManagement.Client.Services.Finance
{
    public class TransactionDataService : ITransactionDataService
    {
        protected HttpClient httpClient { get; }
        protected string ApiControllerName { get; set; } = string.Empty;

        public TransactionDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            ApiControllerName = "Transaction";
        }

        public async Task<List<TransactionDetailResponseModel>> GetDetailByAccountTypeAsync(int accountTypeId)
        {
            try
            {
                var returnVal = await httpClient.GetFromJsonAsync<IEnumerable<TransactionDetailResponseModel>>($"api/{ApiControllerName}/GetTransactionsByAccountType/{accountTypeId}");
                return returnVal != null ? returnVal.ToList() : new List<TransactionDetailResponseModel>();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
