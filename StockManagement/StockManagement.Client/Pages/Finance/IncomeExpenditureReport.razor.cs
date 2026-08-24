using StockManagement.Models.Dto.Finance;

namespace StockManagement.Client.Pages.Finance
{
    public partial class IncomeExpenditureReportBase : FinanceReportBase
    {
        protected List<IncomeExpenditureDto> Items = new();

        protected override async Task LoadReportDataAsync()
        {
            Items = await ReportDataService.GetIncomeExpenditureReportAsync(FromDate, ToDate, 0);
        }

        protected IEnumerable<IGrouping<string, IncomeExpenditureDto>> AccountsIn(int sectionId)
            => Items.Where(i => i.SectionId == sectionId).GroupBy(i => i.AccountName);

        protected bool HasSection(int sectionId) => Items.Any(i => i.SectionId == sectionId);

        protected decimal SectionTotal(int sectionId)
            => Items.Where(i => i.SectionId == sectionId).Sum(i => i.Amount);

        protected decimal NonCashTotal => Items.Where(i => i.IsNonCash).Sum(i => i.Amount);

        protected string PdfUrl => $"api/Pdf/income-expenditure?{PeriodQuery}";
    }
}
