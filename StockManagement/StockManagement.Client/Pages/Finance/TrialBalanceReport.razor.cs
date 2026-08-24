using StockManagement.Models.Dto.Finance;

namespace StockManagement.Client.Pages.Finance
{
    public partial class TrialBalanceReportBase : FinanceReportBase
    {
        protected List<TrialBalanceDto> Items = new();

        protected override async Task LoadReportDataAsync()
        {
            Items = await ReportDataService.GetTrialBalanceReportAsync(FromDate, ToDate);
        }

        protected decimal TotalDebit => Items.Sum(i => i.Debit);
        protected decimal TotalCredit => Items.Sum(i => i.Credit);
        protected decimal TotalBalanceDebit => Items.Sum(i => i.BalanceDebit);
        protected decimal TotalBalanceCredit => Items.Sum(i => i.BalanceCredit);

        protected bool Balances => Math.Round(TotalDebit, 2) == Math.Round(TotalCredit, 2);

        protected string PdfUrl => $"api/Pdf/trial-balance?{PeriodQuery}";
    }
}
