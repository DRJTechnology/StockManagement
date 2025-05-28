using AutoMapper;
using StockManagement.Models.Dto.Reports;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class ReportService(IMapper mapper, IReportRepository reportRepository) : IReportService
    {
        public async Task<List<SalesReportItemDto>> GetSalesReportAsync(int venueId, int productTypeId, int productId)
        {
            var reportItems = mapper.Map<List<SalesReportItemDto>>(await reportRepository.GetSalesReportAsync(venueId, productTypeId, productId));
            return reportItems;
        }

        public async Task<List<StockReportItemDto>> GetStockReportAsync(int venueId, int productTypeId, int productId)
        {
            var reportItems = mapper.Map<List<StockReportItemDto>>(await reportRepository.GetStockReportAsync(venueId, productTypeId, productId));
            return reportItems;
        }
    }
}
