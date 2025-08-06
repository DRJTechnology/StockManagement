using StockManagement.Client.Interfaces.Finance;
using StockManagement.Models;
using StockManagement.Models.Finance;
using StockManagement.Models.InternalObjects;
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

        public async Task<TransactionFilteredResponseModel> GetFilteredAsync(TransactionFilterModel transactionFilterModel)
        {
            try
            {
                var query = new List<string>();

                if (transactionFilterModel.ToDate.HasValue)
                    query.Add($"ToDate={transactionFilterModel.ToDate.Value:yyyy-MM-dd}");
                if (transactionFilterModel.FromDate.HasValue)
                    query.Add($"FromDate={transactionFilterModel.FromDate.Value:yyyy-MM-dd}");
                if (transactionFilterModel.AccountId.HasValue)
                    query.Add($"AccountId={transactionFilterModel.AccountId}");
                if (transactionFilterModel.TransactionTypeId.HasValue)
                    query.Add($"TransactionTypeId={transactionFilterModel.TransactionTypeId}");
                query.Add($"CurrentPage={transactionFilterModel.CurrentPage}");
                query.Add($"PageSize={transactionFilterModel.PageSize}");

                var queryString = string.Join("&", query);

                var url = $"api/{ApiControllerName}/GetFiltered?{queryString}";

                return await httpClient.GetFromJsonAsync<TransactionFilteredResponseModel>(url);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> CreateExpenseIncomeAsync(TransactionDetailEditModel editTransactionDetail)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<TransactionDetailEditModel>($"api/{ApiControllerName}/CreateExpenseIncome", editTransactionDetail);
                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to create expense/income.");
                }

                var returnValue = await response.Content.ReadFromJsonAsync<ApiResponse>();

                return returnValue.CreatedId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateExpenseIncomeAsync(TransactionDetailEditModel editTransactionDetail)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync<TransactionDetailEditModel>($"api/{ApiControllerName}/UpdateExpenseIncome", editTransactionDetail);
                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to update expense/income.");
                }

                var returnValue = await response.Content.ReadFromJsonAsync<ApiResponse>();

                return returnValue.CreatedId > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteByDetailIdAsync(int transactionDetailId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/{ApiControllerName}/DeleteByDetailId/{transactionDetailId}");
                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to delete {ApiControllerName}.");
                }

                var returnValue = await response.Content.ReadFromJsonAsync<ApiResponse>();

                return returnValue.Success;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
