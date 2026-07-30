using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;
using StockManagement.Models.Dto.Reports;

[Authorize]
public partial class SalesReportBase : ComponentBase
{
    [Inject]
    protected IReportDataService ReportDataService { get; set; } = default!;
 
    [Inject]
    protected ILookupsDataService LookupsService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    public LookupsModel Lookups { get; private set; } = new LookupsModel();

    protected bool IsLoading = true;
    private int _salesReportType = 1;
    private int _locationId;
    private int _customerId;
    private int _productTypeId;
    private int _productId;
        protected bool ShowNotesPanel { get; set; } = false;
    protected string SelectedLocationNotes { get; set; } = string.Empty;
    protected string SelectedLocationTitle { get; set; } = string.Empty;

    protected int SalesReportType
    {
        get => _salesReportType;
        set
        {
            if (_salesReportType != value)
            {
                _salesReportType = value;
                _ = PopulateReport();
            }
        }
    }

    protected int LocationId
    {
        get => _locationId;
        set
        {
            if (_locationId != value)
            {
                _locationId = value;
                _ = PopulateReport();
            }
        }
    }

    protected int CustomerId
    {
        get => _customerId;
        set
        {
            if (_customerId != value)
            {
                _customerId = value;
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

    protected List<SalesReportItemDto> SalesReportItems = new();
    protected Dictionary<string, Dictionary<string, List<SalesReportItemDto>>> GroupedSalesReport = new();

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

        SalesReportItems = await ReportDataService.GetSalesReportAsync(SalesReportType, LocationId, CustomerId, ProductTypeId, ProductId);

        // Group by Customer when the report is run by customer, otherwise by Location.
        Func<SalesReportItemDto, string> groupSelector = SalesReportType == 2
            ? item => item.CustomerName
            : item => item.LocationName;

        GroupedSalesReport = SalesReportItems
            .GroupBy(groupSelector)
            .ToDictionary(
                locationGroup => locationGroup.Key,
                locationGroup => locationGroup
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
    protected void ShowLocationNotes(string venuName)
    {
        ShowNotesPanel = true;
        var selectedLocation = Lookups.LocationList
            .FirstOrDefault(v => v.Name.Equals(venuName, StringComparison.OrdinalIgnoreCase));
        SelectedLocationNotes = string.IsNullOrEmpty(selectedLocation?.Notes) ? "NO NOTES" : selectedLocation.Notes;
        SelectedLocationTitle = selectedLocation.Name;
    }
    protected void CloseNotesPanel()
    {
        ShowNotesPanel = false;
    }

}

