using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;

[Authorize]
public partial class StockSaleListBase : ComponentBase
{
    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    [Inject]
    protected IStockSaleDataService StockSaleService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected List<StockSaleResponseModel> StockSales { get; set; } = new();
    protected bool IsLoading { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadStockSales();
        }
    }

    protected async Task LoadStockSales()
    {
        StockSales = (await StockSaleService.GetAllAsync())?.ToList() ?? new();
        IsLoading = false;
    }

    protected void AddStockSale()
    {
        Navigation.NavigateTo("/stock-sale/0");
    }

    protected void ViewDetails(StockSaleResponseModel note)
    {
        Navigation.NavigateTo($"/stock-sale/{note.Id}");
    }

}
