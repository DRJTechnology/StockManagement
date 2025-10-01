using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Dto.Reports;

[Authorize]
public partial class TrialBalanceReportBase : ComponentBase
{
    [Inject]
    protected IReportDataService ReportDataService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    protected IJavascriptMethodsService JavascriptMethodsService { get; set; } = default!;

    protected bool IsLoading = true;

    protected List<TrialBalanceDto> TrialBalanceReportItems = new();

    protected override async Task OnInitializedAsync()
    {
        if (JSRuntime is IJSInProcessRuntime)
        {
            await PopulateReport();
        }
    }

    protected async Task PopulateReport()
    {
        IsLoading = true;
        TrialBalanceReportItems = await ReportDataService.GetTrialBalanceReportAsync();
        IsLoading = false;
        StateHasChanged();
    }
}
