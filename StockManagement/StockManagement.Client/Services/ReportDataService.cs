using StockManagement.Client.Interfaces;
using StockManagement.Client.Pages;
using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Dto.Reports;
using System.Net.Http.Json;

namespace StockManagement.Client.Services
{
    public class ReportDataService : IReportDataService
    {
        protected HttpClient httpClient { get; }
        protected ErrorNotificationService ErrorService { get; }
        protected string ApiControllerName { get; set; } = "Report";

        public ReportDataService(HttpClient httpClient, ErrorNotificationService errorService)
        {
            this.httpClient = httpClient;
            ErrorService = errorService;
        }

        public async Task<List<SalesReportItemDto>> GetSalesReportAsync(int locationId, int productTypeId, int productId)
        {
            try
            {
                var returnVal = await httpClient.GetFromJsonAsync<List<SalesReportItemDto>>($"api/{ApiControllerName}/sales?locationId={locationId}&productTypeId={productTypeId}&productId={productId}");
                return returnVal;
            }
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }

        public async Task<List<StockReportItemDto>> GetStockReportAsync(int locationId, int productTypeId, int productId)
        {
            try
            {
                var returnVal = await httpClient.GetFromJsonAsync<List<StockReportItemDto>>($"api/{ApiControllerName}/stock?locationId={locationId}&productTypeId={productTypeId}&productId={productId}");
                return returnVal;
            }
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }

        public async Task<List<BalanceSheetDto>> GetBalanceSheetReportAsync()
        {
            try
            {
                var returnVal = await httpClient.GetFromJsonAsync<List<BalanceSheetDto>>($"api/{ApiControllerName}/balancesheet");
                return returnVal;
            }
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }

        public async Task<List<TrialBalanceDto>> GetTrialBalanceReportAsync()
        {
            try
            {
                var returnVal = await httpClient.GetFromJsonAsync<List<TrialBalanceDto>>($"api/{ApiControllerName}/trialbalance");
                return returnVal;
            }
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }

        public async Task<List<ProfitAndLossDto>> GetProfitAndLossReportAsync()
        {
            try
            {
                var returnVal = await httpClient.GetFromJsonAsync<List<ProfitAndLossDto>>($"api/{ApiControllerName}/profitandloss");
                return returnVal;
            }
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }

        public async Task<InventoryValueDto> GetInventoryValueReportAsync()
        {
            try
            {
                var returnVal = await httpClient.GetFromJsonAsync<InventoryValueDto>($"api/{ApiControllerName}/inventoryvalue");
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
