using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models.Dto.Finance;

namespace StockManagement.Client.Pages.Finance
{
    /// <summary>
    /// Shared behaviour for the year-end finance reports: the reporting period,
    /// the accounting basis, and loading the data once the WebAssembly runtime
    /// is live. These pages are WebAssembly-only, so they deliberately do not
    /// load during prerender.
    /// </summary>
    [Authorize]
    public abstract class FinanceReportBase : ComponentBase
    {
        [Inject] protected IReportDataService ReportDataService { get; set; } = default!;
        [Inject] public IJSRuntime JSRuntime { get; set; } = default!;

        protected bool IsLoading = true;

        protected DateTime FromDate { get; set; }
        protected DateTime ToDate { get; set; }
        protected AccountingBasis Basis { get; set; } = AccountingBasis.Accruals;

        /// <summary>Period label for on-screen headings, e.g. "6 April 2025 to 5 April 2026".</summary>
        protected string PeriodLabel => $"{FromDate:d MMMM yyyy} to {ToDate:d MMMM yyyy}";

        /// <summary>Query string fragment shared by the report and its PDF endpoint.</summary>
        protected string PeriodQuery =>
            $"fromDate={FromDate:yyyy-MM-dd}&toDate={ToDate:yyyy-MM-dd}";

        protected string BasisLabel =>
            Basis == AccountingBasis.Cash ? "Cash basis" : "Accruals basis";

        protected override async Task OnInitializedAsync()
        {
            // Default to the year the accounts are most likely being prepared for.
            var (from, to) = FinancialYear.MostRecentlyCompleted(DateTime.Today);
            FromDate = from;
            ToDate = to;

            if (JSRuntime is IJSInProcessRuntime)
            {
                await PopulateReport();
            }
        }

        protected async Task ReloadAsync() => await PopulateReport();

        protected async Task PopulateReport()
        {
            IsLoading = true;
            StateHasChanged();
            try
            {
                await LoadReportDataAsync();
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        protected abstract Task LoadReportDataAsync();
    }
}
