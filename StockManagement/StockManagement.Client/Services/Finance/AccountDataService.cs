using StockManagement.Client.Interfaces.Finance;
using StockManagement.Models.Finance;
using System.Net.Http.Json;

namespace StockManagement.Client.Services.Finance
{
    public class AccountDataService : GenericDataService<AccountEditModel, AccountResponseModel>, IAccountDataService
    {
        public AccountDataService(HttpClient httpClient, ErrorNotificationService errorService)
            : base(httpClient, errorService)
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
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AccountResponseModel>> GetByTypeAsync(int accountTypeId)
        {
            try
            {
                var returnVal = await httpClient.GetFromJsonAsync<IEnumerable<AccountResponseModel>>($"api/{ApiControllerName}/GetByType/{accountTypeId}");
                return returnVal;
            }
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }
    }
}
