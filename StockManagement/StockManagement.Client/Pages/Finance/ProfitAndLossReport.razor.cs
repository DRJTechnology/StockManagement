using StockManagement.Models.Dto.Finance;

namespace StockManagement.Client.Pages.Finance
{
    public partial class ProfitAndLossReportBase : FinanceReportBase
    {
        protected List<ProfitAndLossDto> Items = new();

        protected override async Task LoadReportDataAsync()
        {
            Items = await ReportDataService.GetProfitAndLossReportAsync(FromDate, ToDate, Basis);
        }

        protected decimal Income => Items.Where(i => i.SectionId == 1).Sum(i => i.Amount);
        protected decimal CostOfSales => Items.Where(i => i.SectionId == 2).Sum(i => i.Amount);
        protected decimal Expenses => Items.Where(i => i.SectionId == 3).Sum(i => i.Amount);

        protected decimal GrossProfit => Income - CostOfSales;

        /// <summary>
        /// Income less cost of sales less expenses. Never a plain sum of every
        /// row: the stored procedure returns all amounts positive.
        /// </summary>
        protected decimal NetProfit => Income - CostOfSales - Expenses;

        protected IEnumerable<ProfitAndLossDto> Section(int sectionId)
            => Items.Where(i => i.SectionId == sectionId);

        protected string PdfUrl => $"api/Pdf/profit-and-loss?{PeriodQuery}&basis={(int)Basis}";
    }
}
