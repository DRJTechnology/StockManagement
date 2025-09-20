using AutoMapper;
using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Dto.Reports;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class ReportService(IMapper mapper, IReportRepository reportRepository) : IReportService
    {
        public async Task<List<BalanceSheetDto>> GetBalanceSheetReportAsync()
        {
            var reportItems = mapper.Map<List<BalanceSheetDto>>(await reportRepository.GetBalanceSheetReportAsync());
            return reportItems;
        }

        public async Task<List<SalesReportItemDto>> GetSalesReportAsync(int locationId, int productTypeId, int productId)
        {
            var reportItems = mapper.Map<List<SalesReportItemDto>>(await reportRepository.GetSalesReportAsync(locationId, productTypeId, productId));
            return reportItems;
        }

        public async Task<List<StockReportItemDto>> GetStockReportAsync(int locationId, int productTypeId, int productId)
        {
            var reportItems = mapper.Map<List<StockReportItemDto>>(await reportRepository.GetStockReportAsync(locationId, productTypeId, productId));
            return reportItems;
        }
    }
}
