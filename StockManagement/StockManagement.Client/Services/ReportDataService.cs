using StockManagement.Client.Interfaces;
using StockManagement.Models.Dto.Reports;
using System.Net.Http.Json;

namespace StockManagement.Client.Services
{
    public class ReportDataService : IReportDataService
    {
        protected HttpClient httpClient { get; }
        protected string ApiControllerName { get; set; } = "Report";

        public ReportDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<SalesReportItemDto>> GetSalesReportAsync(int venueId, int productTypeId, int productId)
        {
            try
            {
                var returnVal = await httpClient.GetFromJsonAsync<List<SalesReportItemDto>>($"api/{ApiControllerName}/sales?venueId={venueId}&productTypeId={productTypeId}&productId={productId}");
                return returnVal;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StockReportItemDto>> GetStockReportAsync(int venueId, int productTypeId, int productId)
        {
            var returnVal = await httpClient.GetFromJsonAsync<List<StockReportItemDto>>($"api/{ApiControllerName}/stock?venueId={venueId}&productTypeId={productTypeId}&productId={productId}");
            return returnVal;
        }
    }
}
