using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class SettingDataService : GenericDataService<SettingEditModel, SettingResponseModel>, ISettingDataService
    {
        public SettingDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "Setting";
        }
    }
}
