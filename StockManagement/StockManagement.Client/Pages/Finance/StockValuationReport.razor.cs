using StockManagement.Models.Dto.Finance;

namespace StockManagement.Client.Pages.Finance
{
    public partial class StockValuationReportBase : FinanceReportBase
    {
        protected List<StockValuationDto> Items = new();

        protected override async Task LoadReportDataAsync()
        {
            // Stock valuation is a position at a date, so only ToDate applies.
            Items = await ReportDataService.GetStockValuationReportAsync(ToDate, 0, 0, 0);
        }

        protected int TotalQuantity => Items.Sum(i => i.Quantity);
        protected decimal TotalCost => Items.Sum(i => i.CostValue);
        protected decimal TotalMarket => Items.Sum(i => i.MarketValue);

        protected IEnumerable<IGrouping<string, StockValuationDto>> ByLocation
            => Items.GroupBy(i => i.LocationName);

        /// <summary>
        /// Items held at nil cost - typically originals the artist produced
        /// herself, where the materials were expensed as they were bought. The
        /// accountant needs to see these separately.
        /// </summary>
        protected int NilCostQuantity => Items.Where(i => i.CostValue == 0).Sum(i => i.Quantity);

        protected decimal NilCostMarketValue => Items.Where(i => i.CostValue == 0).Sum(i => i.MarketValue);

        protected string PdfUrl => $"api/Pdf/stock-valuation?asAtDate={ToDate:yyyy-MM-dd}";
    }
}
