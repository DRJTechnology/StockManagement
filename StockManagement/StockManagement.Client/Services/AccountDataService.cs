using StockManagement.Client.Interfaces;
using StockManagement.Models.Finance;
using System.Net.Http.Json;

namespace StockManagement.Client.Services
{
    public class AccountDataService : GenericDataService<AccountEditModel, AccountResponseModel>, IAccountDataService
    {
        public AccountDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "Account";
        }

        public async Task<IEnumerable<AccountResponseModel>> GetAllAsync(bool includeInactive)
        {
            try
            {
                var returnVal = await httpClient.GetFromJsonAsync<IEnumerable<AccountResponseModel>>($"api/{ApiControllerName}/GetAll/{includeInactive}");
                return returnVal;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
