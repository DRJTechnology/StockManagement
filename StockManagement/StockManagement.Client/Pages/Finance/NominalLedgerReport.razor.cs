using StockManagement.Models.Dto.Finance;

namespace StockManagement.Client.Pages.Finance
{
    public partial class NominalLedgerReportBase : FinanceReportBase
    {
        protected List<NominalLedgerDto> Items = new();

        protected override async Task LoadReportDataAsync()
        {
            Items = await ReportDataService.GetNominalLedgerReportAsync(FromDate, ToDate, 0);
        }

        protected IEnumerable<IGrouping<string, NominalLedgerDto>> ByAccount
            => Items.GroupBy(i => i.AccountName);

        protected string PdfUrl => $"api/Pdf/nominal-ledger?{PeriodQuery}";
    }
}
