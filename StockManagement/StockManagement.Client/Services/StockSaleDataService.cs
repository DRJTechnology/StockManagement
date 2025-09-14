using StockManagement.Client.Interfaces;
using StockManagement.Models;
using StockManagement.Models.InternalObjects;
using System.Net.Http.Json;

namespace StockManagement.Client.Services
{
    public class StockSaleDataService : GenericDataService<StockSaleEditModel, StockSaleResponseModel>, IStockSaleDataService
    {
        public StockSaleDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "StockSale";
        }
        public async Task<bool> ConfirmStockSale(StockSaleConfirmationModel stockSaleConfirmation)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<StockSaleConfirmationModel>($"api/{ApiControllerName}/ConfirmStockSale", stockSaleConfirmation);
                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to confirm stock sale.");
                }

                var returnValue = await response.Content.ReadFromJsonAsync<ApiResponse>();

                return returnValue.Success;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ConfirmStockSalePayment(StockSaleConfirmPaymentModel stockSaleConfirmPaymentModel)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<StockSaleConfirmPaymentModel>($"api/{ApiControllerName}/ConfirmStockSalePayment", stockSaleConfirmPaymentModel);
                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to confirm stock sale payment.");
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
