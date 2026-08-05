using Microsoft.AspNetCore.Components;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;

public partial class ReportFilterBase : ComponentBase
{
    [Parameter] public LookupsModel Lookups { get; set; } = new LookupsModel();

    /// <summary>
    /// When true, a "Group By" selector (Location / Customer) and a Customer
    /// filter are shown. Used by the Sales Report; other reports leave this off
    /// and only ever filter by Location.
    /// </summary>
    [Parameter] public bool ShowReportTypeSelector { get; set; } = false;

    [Parameter] public EventCallback<int> SalesReportTypeChanged { get; set; }

    [Parameter] public EventCallback<int> LocationIdChanged { get; set; }

    [Parameter] public EventCallback<int> CustomerIdChanged { get; set; }

    [Parameter] public EventCallback<int> ProductTypeIdChanged { get; set; }

    [Parameter] public EventCallback<int> ProductIdChanged { get; set; }

    /// <summary>
    /// When true, From/To date inputs are shown. Used by the Sales Report so it
    /// can be run for a financial year; the Stock Report is a position at the
    /// current moment and has no date range.
    /// </summary>
    [Parameter] public bool ShowDateRange { get; set; } = false;

    [Parameter] public EventCallback<DateTime> FromDateChanged { get; set; }

    [Parameter] public EventCallback<DateTime> ToDateChanged { get; set; }

    private int _salesReportType = 1;
    private int _locationId;
    private int _customerId;
    private int _productTypeId;
    private int _productd;
    private DateTime _fromDate = new DateTime(2000, 1, 1);
    private DateTime _toDate = new DateTime(2099, 12, 31);

    protected bool filtersExpanded = true;

    [Parameter]
    public int SalesReportType
    {
        get => _salesReportType;
        set
        {
            if (_salesReportType != value)
            {
                _salesReportType = value;
                _ = SalesReportTypeChanged.InvokeAsync(_salesReportType);
            }
        }
    }

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
    public int CustomerId
    {
        get => _customerId;
        set
        {
            if (_customerId != value)
            {
                _customerId = value;
                _ = CustomerIdChanged.InvokeAsync(_customerId);
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

    [Parameter]
    public DateTime FromDate
    {
        get => _fromDate;
        set
        {
            if (_fromDate != value)
            {
                _fromDate = value;
                _ = FromDateChanged.InvokeAsync(_fromDate);
            }
        }
    }

    [Parameter]
    public DateTime ToDate
    {
        get => _toDate;
        set
        {
            if (_toDate != value)
            {
                _toDate = value;
                _ = ToDateChanged.InvokeAsync(_toDate);
            }
        }
    }

    /// <summary>
    /// Sets the range to the most recently completed UK tax year, which is the
    /// period the accounts are normally being prepared for.
    /// </summary>
    protected void SetTaxYear()
    {
        var (from, to) = FinancialYear.MostRecentlyCompleted(DateTime.Today);
        FromDate = from;
        ToDate = to;
    }

    protected void ClearDateRange()
    {
        FromDate = new DateTime(2000, 1, 1);
        ToDate = new DateTime(2099, 12, 31);
    }

    protected void ToggleFilters()
    {
        filtersExpanded = !filtersExpanded;
    }
}
