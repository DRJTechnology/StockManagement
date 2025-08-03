using StockManagement.Client.Interfaces;
using StockManagement.Models;
using System.Net.Http.Json;

namespace StockManagement.Client.Services
{
    public class ContactDataService : GenericDataService<ContactEditModel, ContactResponseModel>, IContactDataService
    {
        public ContactDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "Contact";
        }
        public async Task<IEnumerable<ContactResponseModel>> GetByTypeAsync(int contactTypeId)
        {
            try
            {
                var returnVal = await httpClient.GetFromJsonAsync<IEnumerable<ContactResponseModel>>($"api/{ApiControllerName}/GetByType/{contactTypeId}");
                return returnVal;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
