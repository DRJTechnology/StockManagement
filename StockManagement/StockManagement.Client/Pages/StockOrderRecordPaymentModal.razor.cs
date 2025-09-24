using Microsoft.AspNetCore.Components;
using StockManagement.Models;

public partial class StockOrderRecordPaymentModalBase : ComponentBase
{
    [Parameter] public bool Show { get; set; }

    //protected List<StockOrderDetailPaymentResponseModel> _detailList;
    protected StockOrderPaymentsCreateModel _paymentDetail;
    //[Parameter] public List<StockOrderDetailPaymentResponseModel> DetailList
    //{
    //    get
    //    {
    //        return _detailList;
    //    }
    //    set
    //    {
    //        _detailList = value;
    //        CalculatedTotal = _detailList.Sum(d => d.Total);
    //    } 
    //}

    [Parameter] public StockOrderPaymentsCreateModel PaymentDetail
    {
        get
        {
            return _paymentDetail;
        }
        set
        {
            _paymentDetail = value;
            CalculatedTotal = _paymentDetail.StockOrderDetailPayments.Sum(d => d.Total);
        }
    }

    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback OnValidSubmit { get; set; }
    [Parameter] public EventCallback OnRecordPayment { get; set; }


    protected decimal CalculatedTotal { get; set; }
    protected decimal TotalPurchasePrice { get; set; }

    protected override void OnInitialized()
    {
        TotalPurchasePrice = Math.Round(TotalPurchasePrice, 2);
    }

    protected async Task HandleValidSubmit()
    {
        await OnValidSubmit.InvokeAsync();
    }

    protected void CalculatePricing()
    {
        _paymentDetail.Cost = TotalPurchasePrice;
        foreach (var item in _paymentDetail.StockOrderDetailPayments)
        {
            var percentageShare = item.Total / CalculatedTotal;
            item.Total = TotalPurchasePrice * percentageShare;
            item.UnitPrice = item.Total / item.Quantity;
        }
        CalculatedTotal = _paymentDetail.StockOrderDetailPayments.Sum(d => d.Total);
        //CalculatedTotal = Math.Round(CalculatedTotal, 2);
    }
    protected bool DisableRecordPayment()
    { 
        return TotalPurchasePrice == 0 || TotalPurchasePrice != CalculatedTotal;
    }
}
