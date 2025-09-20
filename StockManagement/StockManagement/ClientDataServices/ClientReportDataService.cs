using StockManagement.Client.Interfaces;
using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Dto.Reports;

namespace StockManagement.ClientDataServices
{
    public class ClientReportDataService : IReportDataService
    {
        public Task<List<BalanceSheetDto>> GetBalanceSheetReportAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<SalesReportItemDto>> GetSalesReportAsync(int locationId, int productTypeId, int productId)
        {
            throw new NotImplementedException();
        }

        public Task<List<StockReportItemDto>> GetStockReportAsync(int locationId, int productTypeId, int productId)
        {
            throw new NotImplementedException();
        }
    }
}
