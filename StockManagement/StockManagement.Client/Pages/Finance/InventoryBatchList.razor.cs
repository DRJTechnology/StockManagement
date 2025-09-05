using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Enums;
using StockManagement.Models.Finance;
using System.Diagnostics;

[Authorize]
public partial class InventoryBatchListBase : ComponentBase
{
    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    [Inject]
    protected IInventoryBatchDataService InventoryBatchService { get; set; } = default!;

    [Inject]
    protected ILookupsDataService LookupsService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    protected IJavascriptMethodsService JavascriptMethodsService { get; set; } = default!;

    protected List<InventoryBatchResponseModel> InventoryBatchList { get; set; } = new();
    protected List<InventoryBatchActivityDto> InventoryBatchActivity {  get; set; } = new();
    protected bool IsLoading { get; set; }
    protected bool filtersExpanded = false;
    protected bool showActivityHistory = false;

    public LookupsModel Lookups { get; private set; }

    protected InventoryBatchFilterModel inventoryBatchFilterModel = new InventoryBatchFilterModel() { Status = InventoryBatchStatusEnum.Active };

    protected int TotalPages { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadInventoryBatchList();
            await LoadLookups();
            IsLoading = false;
        }
        System.Diagnostics.Debug.WriteLine("Loaded!");

    }

    protected async Task LoadInventoryBatchList()
    {
        var filteredActivities = await InventoryBatchService.GetFilteredAsync(inventoryBatchFilterModel);

        InventoryBatchList = filteredActivities.InventoryBatchList ?? new List<InventoryBatchResponseModel>();

        TotalPages = filteredActivities.TotalPages;
        if (inventoryBatchFilterModel.CurrentPage > TotalPages && TotalPages > 0)
            inventoryBatchFilterModel.CurrentPage = TotalPages;
        if (inventoryBatchFilterModel.CurrentPage < 1)
            inventoryBatchFilterModel.CurrentPage = 1;
    }

    protected async Task OnFilter()
    {
        inventoryBatchFilterModel.CurrentPage = 1;
        await LoadInventoryBatchList();
    }

    protected async Task OnReset()
    {
        inventoryBatchFilterModel.Status = InventoryBatchStatusEnum.Active;
        inventoryBatchFilterModel.ProductTypeId = null;
        inventoryBatchFilterModel.ProductId = null;
        inventoryBatchFilterModel.LocationId = null;
        inventoryBatchFilterModel.PurchaseDate = null;
        inventoryBatchFilterModel.CurrentPage = 1;
        await LoadInventoryBatchList();
    }

    private async Task LoadLookups()
    {
        var lookupsList = await LookupsService.GetAllAsync();
        Lookups = lookupsList.FirstOrDefault();
    }

    protected async Task GoToPage(int page)
    {
        if (page < 1 || page > TotalPages)
            return;
        inventoryBatchFilterModel.CurrentPage = page;
        await LoadInventoryBatchList();
    }

    protected void ToggleFilters()
    {
        filtersExpanded = !filtersExpanded;
    }

    protected async Task ShowHistory(int inventoryBatchId)
    {
        InventoryBatchActivity = await InventoryBatchService.GetActivityAsync(inventoryBatchId);
        showActivityHistory = true;
    }
    protected void OnCancelHistory(MouseEventArgs args)
    {
        showActivityHistory = false;
    }

    protected void OpenStockSale(int stockSaleId)
    {
        Navigation.NavigateTo($"/stock-sale/{stockSaleId}");
    }

    protected void OpenStockOrder(int stockReceiptId)
    {
        Navigation.NavigateTo($"/stock-order/{stockReceiptId}");
    }

}
