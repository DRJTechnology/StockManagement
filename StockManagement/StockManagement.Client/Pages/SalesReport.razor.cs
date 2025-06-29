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
    private int _venueId;
    private int _productTypeId;
    private int _productId;
        protected bool ShowNotesPanel { get; set; } = false;
    protected string SelectedVenueNotes { get; set; } = string.Empty;
    protected string SelectedVenueTitle { get; set; } = string.Empty;

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

        SalesReportItems = await ReportDataService.GetSalesReportAsync(VenueId, ProductTypeId, ProductId);
        GroupedSalesReport = SalesReportItems
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
    protected void ShowVenueNotes(string venuName)
    {
        ShowNotesPanel = true;
        var selectedVenue = Lookups.VenueList
            .FirstOrDefault(v => v.VenueName.Equals(venuName, StringComparison.OrdinalIgnoreCase));
        SelectedVenueNotes = string.IsNullOrEmpty(selectedVenue?.Notes) ? "NO NOTES" : selectedVenue.Notes;
        SelectedVenueTitle = selectedVenue.VenueName;
    }
    protected void CloseNotesPanel()
    {
        ShowNotesPanel = false;
    }

}

