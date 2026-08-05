using Microsoft.AspNetCore.Components;
using StockManagement.Models.Dto.Finance;

namespace StockManagement.Client.Components.Finance
{
    /// <summary>
    /// Period and basis selector shared by the year-end reports, with a
    /// one-click jump to the most recently completed UK tax year - which is
    /// almost always the period the accounts are being prepared for.
    /// </summary>
    public partial class ReportPeriodFilterBase : ComponentBase
    {
        [Parameter] public DateTime FromDate { get; set; }
        [Parameter] public EventCallback<DateTime> FromDateChanged { get; set; }

        [Parameter] public DateTime ToDate { get; set; }
        [Parameter] public EventCallback<DateTime> ToDateChanged { get; set; }

        [Parameter] public AccountingBasis Basis { get; set; } = AccountingBasis.Accruals;
        [Parameter] public EventCallback<AccountingBasis> BasisChanged { get; set; }

        /// <summary>False for as-at-a-date reports such as the stock valuation.</summary>
        [Parameter] public bool ShowFromDate { get; set; } = true;

        [Parameter] public bool ShowBasis { get; set; }

        [Parameter] public string? PdfUrl { get; set; }

        /// <summary>Raised after any change, so the page can reload its data.</summary>
        [Parameter] public EventCallback OnChanged { get; set; }

        protected string TaxYearLabel { get; private set; } = string.Empty;

        protected override void OnInitialized()
        {
            var (from, _) = FinancialYear.MostRecentlyCompleted(DateTime.Today);
            TaxYearLabel = FinancialYear.Describe(from);
        }

        protected DateTime FromDateValue
        {
            get => FromDate;
            set => _ = SetPeriodAsync(value, ToDate, Basis);
        }

        protected DateTime ToDateValue
        {
            get => ToDate;
            set => _ = SetPeriodAsync(FromDate, value, Basis);
        }

        protected AccountingBasis BasisValue
        {
            get => Basis;
            set => _ = SetPeriodAsync(FromDate, ToDate, value);
        }

        protected Task SetTaxYear()
        {
            var (from, to) = FinancialYear.MostRecentlyCompleted(DateTime.Today);
            return SetPeriodAsync(from, to, Basis);
        }

        private async Task SetPeriodAsync(DateTime from, DateTime to, AccountingBasis basis)
        {
            var changed = false;

            if (from != FromDate)
            {
                FromDate = from;
                await FromDateChanged.InvokeAsync(from);
                changed = true;
            }

            if (to != ToDate)
            {
                ToDate = to;
                await ToDateChanged.InvokeAsync(to);
                changed = true;
            }

            if (basis != Basis)
            {
                Basis = basis;
                await BasisChanged.InvokeAsync(basis);
                changed = true;
            }

            if (changed)
            {
                await OnChanged.InvokeAsync();
            }
        }
    }
}
