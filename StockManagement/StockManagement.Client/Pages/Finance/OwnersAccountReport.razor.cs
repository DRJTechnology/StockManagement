using StockManagement.Models.Dto.Finance;

namespace StockManagement.Client.Pages.Finance
{
    public partial class OwnersAccountReportBase : FinanceReportBase
    {
        protected List<OwnersAccountDto> Items = new();

        protected override async Task LoadReportDataAsync()
        {
            Items = await ReportDataService.GetOwnersAccountReportAsync(FromDate, ToDate);
        }

        protected IEnumerable<IGrouping<string, OwnersAccountDto>> ByAccount
            => Items.GroupBy(i => i.AccountName);

        protected decimal CapitalIntroduced => Items.Where(i => i.AccountId == 3).Sum(i => i.Amount);
        protected decimal Drawings => Items.Where(i => i.AccountId == 4).Sum(i => i.Amount);
        protected decimal NetMovement => CapitalIntroduced + Drawings;

        protected IEnumerable<(string Category, decimal Total, int Count)> ByCategory
            => Items.GroupBy(i => i.Category)
                    .Select(g => (g.Key, g.Sum(i => i.Amount), g.Count()))
                    .OrderBy(g => g.Item1);

        protected string PdfUrl => $"api/Pdf/owners-account?{PeriodQuery}";
    }
}
