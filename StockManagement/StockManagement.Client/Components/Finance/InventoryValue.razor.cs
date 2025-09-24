using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Client.Services;

public partial class InventoryValueBase : ComponentBase
{
    [Inject]
    protected IReportDataService ReportDataService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    public decimal TotalValue { get; set; }
    protected bool IsLoading = true;

    protected override async Task OnInitializedAsync()
    {
        if (JSRuntime is IJSInProcessRuntime)
        {
            await PopulateReport();
        }
    }

    private async Task PopulateReport()
    {
        IsLoading = true;
        TotalValue = await ReportDataService.GetInventoryValueReportAsync();
        IsLoading = false;
        StateHasChanged();
    }
}

