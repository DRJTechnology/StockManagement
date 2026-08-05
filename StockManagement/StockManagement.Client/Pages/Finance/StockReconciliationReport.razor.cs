using StockManagement.Models.Dto.Finance;

namespace StockManagement.Client.Pages.Finance
{
    public partial class StockReconciliationReportBase : FinanceReportBase
    {
        protected List<StockReconciliationDto> Items = new();

        protected override async Task LoadReportDataAsync()
        {
            Items = await ReportDataService.GetStockReconciliationReportAsync(FromDate, ToDate);
        }

        protected decimal Difference =>
            Items.FirstOrDefault(i => i.SortOrder == 10)?.Amount ?? 0;

        protected string PdfUrl => $"api/Pdf/stock-reconciliation?{PeriodQuery}";
    }
}
