using StockManagement.Models.Dto.Finance;

namespace StockManagement.Client.Pages.Finance
{
    public partial class BalanceSheetReportBase : FinanceReportBase
    {
        protected List<BalanceSheetDto> Items = new();

        protected override async Task LoadReportDataAsync()
        {
            Items = await ReportDataService.GetBalanceSheetReportAsync(FromDate, ToDate, Basis);
        }

        protected IEnumerable<BalanceSheetDto> Section(int sectionId)
            => Items.Where(i => i.SectionId == sectionId);

        protected decimal SectionTotal(int sectionId) => Section(sectionId).Sum(i => i.Amount);

        protected decimal NetAssets =>
            SectionTotal(1) - SectionTotal(2) - SectionTotal(3);

        protected decimal TotalCapital => SectionTotal(4);

        /// <summary>
        /// Net assets should always equal total capital. Anything else means a
        /// posting is missing a side, so the report says so rather than quietly
        /// presenting a sheet that does not balance.
        /// </summary>
        protected bool Balances => Math.Round(NetAssets, 2) == Math.Round(TotalCapital, 2);

        protected string PdfUrl => $"api/Pdf/balance-sheet?{PeriodQuery}&basis={(int)Basis}";
    }
}
