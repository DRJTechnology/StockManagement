using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;
using StockManagement.Models.Dto.Reports;

[Authorize]
public partial class StockReportBase : ComponentBase
{

    [Inject]
    protected IReportDataService ReportDataService { get; set; } = default!;

    [Inject]
    protected ILookupsDataService LookupsService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    public LookupsModel Lookups { get; private set; } = new LookupsModel();

    protected bool IsLoading = true;
    private int _venueId;
    private int _productTypeId;
    private int _productId;

    protected int VenueId
    {
        get => _venueId;
        set
        {
            if (_venueId != value)
            {
                _venueId = value;
                _ = PopulateReport();
            }
        }
    }

    protected int ProductTypeId
    {
        get => _productTypeId;
        set
        {
            if (_productTypeId != value)
            {
                _productTypeId = value;
                _ = PopulateReport();
            }
        }
    }

    protected int ProductId
    {
        get => _productId;
        set
        {
            if (_productId != value)
            {
                _productId = value;
                _ = PopulateReport();
            }
        }
    }

    protected List<StockReportItemDto> StockReportItems = new();
    protected Dictionary<string, Dictionary<string, List<StockReportItemDto>>> GroupedStockReport = new();

    protected override async Task OnInitializedAsync()
    {
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadLookups();
            await PopulateReport();
        }
    }

    protected async Task PopulateReport()
    {
        IsLoading = true;

        StockReportItems = await ReportDataService.GetStockReportAsync(VenueId, ProductTypeId, ProductId);
        GroupedStockReport = StockReportItems
            .GroupBy(item => item.VenueName)
            .ToDictionary(
                venueGroup => venueGroup.Key,
                venueGroup => venueGroup
                    .GroupBy(item => item.ProductTypeName)
                    .ToDictionary(
                        productTypeGroup => productTypeGroup.Key,
                        productTypeGroup => productTypeGroup.ToList()
                    )
                );
        IsLoading = false;
        StateHasChanged();
    }

    private async Task LoadLookups()
    {
        var lookupsList = await LookupsService.GetAllAsync();
        Lookups = lookupsList.FirstOrDefault() ?? new LookupsModel();
    }

}
