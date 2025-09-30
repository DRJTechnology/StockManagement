using StockManagement.Client.Interfaces;
using StockManagement.Models;
using StockManagement.Models.Finance;
using StockManagement.Models.InternalObjects;
using System.Net.Http.Json;

namespace StockManagement.Client.Services
{
    public class StockOrderDataService : GenericDataService<StockOrderEditModel, StockOrderResponseModel>, IStockOrderDataService
    {
        public StockOrderDataService(HttpClient httpClient, ErrorNotificationService errorService)
            : base(httpClient, errorService)
        {
            ApiControllerName = "StockOrder";
        }

        public async Task<bool> CreateStockOrderPayments(StockOrderPaymentsCreateModel stockOrderDetailPayments)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<StockOrderPaymentsCreateModel>($"api/{ApiControllerName}/CreateStockOrderPayments", stockOrderDetailPayments);
                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to stock order payments.");
                }

                var returnValue = await response.Content.ReadFromJsonAsync<ApiResponse>();

                return returnValue.Success;
            }
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }

        public async Task<bool> MarkStockReceived(int stockOrderId)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<int>($"api/{ApiControllerName}/MarkStockReceived", stockOrderId);
                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to mark stock as received.");
                }

                var returnValue = await response.Content.ReadFromJsonAsync<ApiResponse>();

                return returnValue.Success;
            }
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }
    }
}
