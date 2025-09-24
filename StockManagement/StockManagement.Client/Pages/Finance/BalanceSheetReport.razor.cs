using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Dto.Reports;

[Authorize]
public partial class BalanceSheetReportBase : ComponentBase
{
    [Inject]
    protected IReportDataService ReportDataService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    protected IJavascriptMethodsService JavascriptMethodsService { get; set; } = default!;

    protected bool IsLoading = true;

    protected List<BalanceSheetDto> BalanceSheetReportItems = new();

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
        BalanceSheetReportItems = await ReportDataService.GetBalanceSheetReportAsync();
        IsLoading = false;
        StateHasChanged();
    }
}
