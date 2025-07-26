using Microsoft.AspNetCore.Components;
using StockManagement.Models;

public partial class ReportFilterBase : ComponentBase
{
    [Parameter] public LookupsModel Lookups { get; set; } = new LookupsModel();

    [Parameter] public EventCallback<int> LocationIdChanged { get; set; }

    [Parameter] public EventCallback<int> ProductTypeIdChanged { get; set; }

    [Parameter] public EventCallback<int> ProductIdChanged { get; set; }

    private int _locationId;
    private int _productTypeId;
    private int _productd;

    protected bool filtersExpanded = true;

    [Parameter]
    public int LocationId
    {
        get => _locationId;
        set
        {
            if (_locationId != value)
            {
                _locationId = value;
                _ = LocationIdChanged.InvokeAsync(_locationId);
            }
        }
    }


    [Parameter]
    public int ProductTypeId
    {
        get => _productTypeId;
        set
        {
            if (_productTypeId != value)
            {
                _productTypeId = value;
                _ = ProductTypeIdChanged.InvokeAsync(_productTypeId);
            }
        }
    }

    [Parameter]
    public int ProductId
    {
        get => _productd; // Removed backing field and used auto property
        set
        {
            if (_productd != value)
            {
                _productd = value;
                _ = ProductIdChanged.InvokeAsync(_productd);
            }
        }
    }

    protected void ToggleFilters()
    {
        filtersExpanded = !filtersExpanded;
    }
}
