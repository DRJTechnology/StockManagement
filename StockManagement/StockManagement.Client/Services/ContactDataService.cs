using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class ContactDataService : GenericDataService<ContactEditModel, ContactResponseModel>, IContactDataService
    {
        public ContactDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "Contact";
        }
    }
}
