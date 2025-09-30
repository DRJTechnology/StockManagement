using StockManagement.Client.Interfaces;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;
using System.Net.Http.Json;

namespace StockManagement.Client.Services
{
    public class InventoryBatchDataService : IInventoryBatchDataService
    {
        protected HttpClient httpClient { get; }
        protected ErrorNotificationService ErrorService { get; }
        protected string ApiControllerName { get; set; } = string.Empty;

        public InventoryBatchDataService(HttpClient httpClient, ErrorNotificationService errorService)
        {
            this.httpClient = httpClient;
            ErrorService = errorService;
            ApiControllerName = "InventoryBatch";
        }

        public async Task<InventoryBatchFilteredResponseModel> GetFilteredAsync(InventoryBatchFilterModel inventoryBatchFilterModel)
        {
            try
            {
                var query = new List<string>();

                query.Add($"Status={inventoryBatchFilterModel.Status}");
                if (inventoryBatchFilterModel.ProductTypeId.HasValue)
                    query.Add($"ProductTypeId={inventoryBatchFilterModel.ProductTypeId}");
                if (inventoryBatchFilterModel.ProductId.HasValue)
                    query.Add($"ProductId={inventoryBatchFilterModel.ProductId}");
                if (inventoryBatchFilterModel.LocationId.HasValue)
                    query.Add($"LocationId={inventoryBatchFilterModel.LocationId}");
                if (inventoryBatchFilterModel.PurchaseDate.HasValue)
                    query.Add($"Date={inventoryBatchFilterModel.PurchaseDate.Value:yyyy-MM-dd}");
                query.Add($"CurrentPage={inventoryBatchFilterModel.CurrentPage}");
                query.Add($"PageSize={inventoryBatchFilterModel.PageSize}");

                var queryString = string.Join("&", query);

                var url = $"api/{ApiControllerName}/GetFiltered?{queryString}";

                return await httpClient.GetFromJsonAsync<InventoryBatchFilteredResponseModel>(url);
            }
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }

        public async Task<List<InventoryBatchActivityDto>> GetActivityAsync(int inventoryBatchId)
        {
            try
            {
                var url = $"api/{ApiControllerName}/GetActivity/{inventoryBatchId}";

                return await httpClient.GetFromJsonAsync<List<InventoryBatchActivityDto>>(url);
            }
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }

        public async Task<decimal> GetSaleCostAsync(int stockSaleId)
        {
            try
            {
                var returnVal = await httpClient.GetFromJsonAsync<decimal>($"api/{ApiControllerName}/GetSaleCost/{stockSaleId}");
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
