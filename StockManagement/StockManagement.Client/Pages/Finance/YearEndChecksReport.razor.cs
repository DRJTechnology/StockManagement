using StockManagement.Models.Dto.Finance;

namespace StockManagement.Client.Pages.Finance
{
    public partial class YearEndChecksReportBase : FinanceReportBase
    {
        protected List<YearEndCheckDto> Items = new();

        protected override async Task LoadReportDataAsync()
        {
            Items = await ReportDataService.GetYearEndChecksReportAsync(FromDate, ToDate);
        }

        protected IEnumerable<IGrouping<string, YearEndCheckDto>> ByCheckType
            => Items.GroupBy(i => i.CheckType);

        protected int ActionCount => Items.Count(i => i.Severity == "Action");

        protected static string RowClass(YearEndCheckDto item)
            => item.Severity == "Action" ? "table-warning" : string.Empty;

        protected string PdfUrl => $"api/Pdf/year-end-checks?{PeriodQuery}";

        /// <summary>Every year-end report merged into one PDF, on the given basis.</summary>
        protected string PackUrl(AccountingBasis basis)
            => $"api/Pdf/year-end-pack?{PeriodQuery}&basis={(int)basis}";
    }
}
