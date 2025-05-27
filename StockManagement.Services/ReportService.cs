using AutoMapper;
using StockManagement.Models.Dto.Reports;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class ReportService(IMapper mapper, IReportRepository reportRepository) : IReportService
    {
        public async Task<List<SalesReportItemDto>> GetSalesReportAsync()
        {
            var reportItems = mapper.Map<List<SalesReportItemDto>>(await reportRepository.GetSalesReportAsync());
            return reportItems;
        }

        public async Task<List<StockReportItemDto>> GetStockReportAsync()
        {
            var reportItems = mapper.Map<List<StockReportItemDto>>(await reportRepository.GetStockReportAsync());
            return reportItems;
        }
    }
}
