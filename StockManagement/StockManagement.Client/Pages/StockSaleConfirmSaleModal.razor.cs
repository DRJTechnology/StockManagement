using Microsoft.AspNetCore.Components;
using StockManagement.Models;

public partial class StockSaleConfirmSaleModalBase : ComponentBase
{
    [Parameter] public bool Show { get; set; }

    protected StockSaleConfirmationModel _stockSaleConfirmation;

    [Parameter]
    public StockSaleConfirmationModel StockSaleConfirmationObject
    {
        get
        {
            return _stockSaleConfirmation;
        }
        set
        {
            _stockSaleConfirmation = value;
            CalculateTotalPrice();
        }
    }

    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback OnValidSubmit { get; set; }
    [Parameter] public EventCallback OnConfirmSale { get; set; }

    protected decimal TotalPurchasePrice { get; set; }
    protected bool SyncProductTypePrices { get; set; } = true;

    protected override void OnInitialized()
    {
        CalculateTotalPrice();
    }

    protected async Task HandleValidSubmit()
    {
        await OnValidSubmit.InvokeAsync();
    }

    protected void OnUnitPriceInput(ChangeEventArgs e, StockSaleDetailResponseModel detail)
    {
        detail.UnitPrice = decimal.TryParse(e.Value?.ToString(), out var price) ? price : (decimal?)null;
        if (SyncProductTypePrices)
        {
            foreach (var item in _stockSaleConfirmation.StockSaleDetails.Where(d => d.ProductTypeId == detail.ProductTypeId && d != detail))
            {
                item.UnitPrice = detail.UnitPrice;
            }
        }
        CalculateTotalPrice();
    }

    protected void CalculateTotalPrice()
    {
        StockSaleConfirmationObject.TotalPrice = _stockSaleConfirmation.StockSaleDetails
                                        .Where(ssd => ssd.UnitPrice.HasValue) // Ensure UnitPrice is not null
                                        .Sum(ssd => (decimal)ssd.UnitPrice!.Value * ssd.Quantity); // Safely access the value
    }
}
