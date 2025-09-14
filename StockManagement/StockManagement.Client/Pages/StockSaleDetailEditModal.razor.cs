using Microsoft.AspNetCore.Components;
using StockManagement.Client.Interfaces;
using StockManagement.Models;
using StockManagement.Models.Dto.Reports;

public partial class StockSaleDetailEditModalBase : ComponentBase
{
    [Parameter] public bool Show { get; set; }
    [Parameter] public int LocationId { get; set; }
    [Parameter] public StockSaleDetailEditModel EditStockSaleDetail { get; set; } = new();
    [Parameter] public LookupsModel Lookups { get; set; } = new();
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback OnValidSubmit { get; set; }

    [Inject]
    protected IReportDataService ReportDataService { get; set; } = default!;

    protected string LocationName { get; set; } = string.Empty;

    protected List<StockReportItemDto> StockReportItems = new();
    protected int availableQuantity = 0;

    public int ProductTypeId
    {
        get => EditStockSaleDetail.ProductTypeId;
        set
        {
            if (EditStockSaleDetail.ProductTypeId != value)
            {
                EditStockSaleDetail.ProductTypeId = value;
                _ = GetAvailableQuantity();
            }
        }
    }

    public int ProductId
    {
        get => EditStockSaleDetail.ProductId;
        set
        {
            if (EditStockSaleDetail.ProductId != value)
            {
                EditStockSaleDetail.ProductId = value;
                _ = GetAvailableQuantity();
            }
        }
    }

    protected async Task HandleValidSubmit()
    {
        await OnValidSubmit.InvokeAsync();
    }

    protected async Task GetAvailableQuantity()
    {
        if (EditStockSaleDetail.ProductTypeId > 0 && EditStockSaleDetail.ProductId > 0)
        {
            StockReportItems = await ReportDataService.GetStockReportAsync(LocationId, EditStockSaleDetail.ProductTypeId, EditStockSaleDetail.ProductId);
            if (StockReportItems.Count > 0)
            {
                availableQuantity = StockReportItems.FirstOrDefault()!.ActiveQuantity;
            }
            else
            {
                availableQuantity = 0;
            }
        }
        else
        {
            availableQuantity = 0;
        }

        LocationName = Lookups.LocationList.FirstOrDefault(l => l.Id == LocationId)?.Name ?? string.Empty;
        await InvokeAsync(StateHasChanged);
    }

    protected async Task Close()
    {
        availableQuantity = 0;
        await OnCancel.InvokeAsync();
    }
}
