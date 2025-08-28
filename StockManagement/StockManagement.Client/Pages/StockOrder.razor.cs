using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;

public partial class StockOrderBase : ComponentBase
{
    [Parameter] public int StockOrderId { get; set; }

    [Inject]
    IJSRuntime JS { get; set; }

    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    [Inject]
    protected IStockOrderDataService StockOrderService { get; set; } = default!;

    [Inject]
    protected IStockOrderDetailDataService StockOrderDetailService { get; set; } = default!;

    [Inject]
    protected ILookupsDataService LookupsService { get; set; } = default!;

    [Inject]
    protected IJavascriptMethodsService JavascriptMethodsService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected LookupsModel Lookups { get; private set; }
    protected bool ShowDeleteReceiptConfirm { get; set; } = false;
    protected bool ShowDeleteDetailConfirm { get; set; } = false;
    protected string SelectedItemName { get; set; } = string.Empty;

    protected bool IsLoading { get; set; }
    protected StockOrderResponseModel EditStockOrder { get; set; } = new();
    //public List<StockOrderDetailPaymentResponseModel> PaymentDetailList { get; set; } = new();
    public StockOrderPaymentsCreateModel PaymentDetail { get; set; } = new();



    protected StockOrderDetailEditModel EditStockOrderDetail { get; set; } = new();

    protected bool ShowEditDetailForm { get; set; }
    protected bool ShowRecordPaymentForm { get; set; }

    protected EditContext editContext;
    protected bool IsFormValid = false;
    protected bool IsDirty = false;
    private bool _postInitActionDone = false;

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadLookups();
            await LoadStockOrder();
        }
        // Initialization done, trigger re-render
        StateHasChanged();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        // Only run after initialization and only once
        if (!_postInitActionDone && editContext != null)
        {
            IsFormValid = editContext.Validate();
            _postInitActionDone = true;
            StateHasChanged();
        }
    }
    protected async Task HandleValidSubmit()
    {
        if (EditStockOrder.Id == 0)
        {
            var newId = await StockOrderService.CreateAsync(EditStockOrder);
            EditStockOrder.Id = newId;
        }
        else
        {
            await StockOrderService.UpdateAsync(EditStockOrder);
        }
        IsDirty = false;
    }

    protected async Task LoadStockOrder()
    {
        if (StockOrderId == 0)
        {
            var localNow = await JavascriptMethodsService.GetLocalDateTimeAsync();
            EditStockOrder = new StockOrderResponseModel() { Date = localNow, ContactId = 0, DetailList = new List<StockOrderDetailResponseModel>() };
        }
        else
        {
            EditStockOrder = await StockOrderService.GetByIdAsync(StockOrderId);
            if (EditStockOrder == null)
            {
                throw new Exception("Invalid Stock Order");
            }
        }
        CreateValidationContext();

        IsLoading = false;
    }

    private void CreateValidationContext()
    {
        editContext = new EditContext(EditStockOrder);
        IsFormValid = editContext.Validate();
        editContext.OnFieldChanged += (_, __) =>
        {
            IsFormValid = editContext.Validate();
            IsDirty = true;
            InvokeAsync(StateHasChanged);
        };
    }

    private async Task LoadLookups()
    {
        var lookupsList = await LookupsService.GetAllAsync();
        Lookups = lookupsList.FirstOrDefault();
    }

    protected async Task ShowAddDetailForm()
    {
        var localNow = await JavascriptMethodsService.GetLocalDateTimeAsync();
        EditStockOrderDetail = new StockOrderDetailEditModel() { StockOrderId = EditStockOrder.Id };
        ShowEditDetailForm = true;
    }

    protected async Task RecordPayment()
    {
        var description = "Purchase of stock (";
        description += string.Join(", ", EditStockOrder.DetailList.Select(d => d.ProductTypeName).Distinct());
        description += ")";

        PaymentDetail = new StockOrderPaymentsCreateModel()
        {
            StockOrderId = EditStockOrder.Id,
            Cost = 0,
            PaymentDate = DateTime.Now,
            Description = description,
            ContactId = EditStockOrder.ContactId,
            StockOrderDetailPayments = EditStockOrder.DetailList
            .Select(detail => new StockOrderDetailPaymentResponseModel
            {
                Id = detail.Id,
                StockOrderId = detail.StockOrderId,
                ProductId = detail.ProductId,
                ProductName = detail.ProductName,
                ProductTypeId = detail.ProductTypeId,
                ProductTypeName = detail.ProductTypeName,
                Quantity = detail.Quantity,
                Deleted = detail.Deleted,
                UnitPrice = Lookups.ProductTypeList.FirstOrDefault(pt => pt.Id == detail.ProductTypeId)!.DefaultCostPrice,
                Total = Lookups.ProductTypeList.FirstOrDefault(pt => pt.Id == detail.ProductTypeId)!.DefaultCostPrice * detail.Quantity,
            })
            .ToList()
        };

        ShowRecordPaymentForm = true;
    }

    protected async Task HandleRecordPayment()
    {
        var response = await StockOrderService.CreateStockOrderPayments(PaymentDetail);
        ShowRecordPaymentForm = false;
    }


    protected async Task ReceiveStock()
    {
    }

    protected void CancelDetailEdit()
    {
        ShowEditDetailForm = false;
    }
    protected void CancelRecordPayment()
    {
        ShowRecordPaymentForm = false;
    }
    protected async Task HandleValidDetailSubmit()
    {
        if (EditStockOrderDetail.Id == 0)
        {
            var newId = await StockOrderDetailService.CreateAsync(EditStockOrderDetail);
            EditStockOrder.DetailList.Add(
                new StockOrderDetailResponseModel()
                {
                    Id = newId,
                    StockOrderId = EditStockOrder.Id,
                    ProductId = EditStockOrderDetail.ProductId,
                    ProductName = Lookups.ProductList.Where(p => p.Id == EditStockOrderDetail.ProductId).FirstOrDefault()!.ProductName,
                    ProductTypeId = EditStockOrderDetail.ProductTypeId,
                    ProductTypeName = Lookups.ProductTypeList.Where(pt => pt.Id == EditStockOrderDetail.ProductTypeId).FirstOrDefault()!.ProductTypeName,
                    Quantity = EditStockOrderDetail.Quantity,
                    Deleted = false
                });
        }
        else
        {
            await StockOrderDetailService.UpdateAsync(EditStockOrderDetail);
            var index = EditStockOrder.DetailList.FindIndex(dnd => dnd.Id == EditStockOrderDetail.Id);
            if (index >= 0)
            {
                EditStockOrder.DetailList[index] = new StockOrderDetailResponseModel()
                {
                    Id = EditStockOrderDetail.Id,
                    StockOrderId = EditStockOrder.Id,
                    ProductId = EditStockOrderDetail.ProductId,
                    ProductName = Lookups.ProductList.Where(p => p.Id == EditStockOrderDetail.ProductId).FirstOrDefault()!.ProductName,
                    ProductTypeId = EditStockOrderDetail.ProductTypeId,
                    ProductTypeName = Lookups.ProductTypeList.Where(pt => pt.Id == EditStockOrderDetail.ProductTypeId).FirstOrDefault()!.ProductTypeName,
                    Quantity = EditStockOrderDetail.Quantity,
                    Deleted = false
                };
            }
        }
        ShowEditDetailForm = false;
    }

    protected void DeleteStockOrder()
    {
        if (EditStockOrder.Id > 0)
        {
            SelectedItemName = "this entire stock order";
            ShowDeleteReceiptConfirm = true;
        }
    }

    protected async Task HandleDeleteConfirmation(bool confirmed)
    {
        if (confirmed && ShowDeleteReceiptConfirm)
        {
            if (confirmed)
            {
                await StockOrderService.DeleteAsync(EditStockOrder.Id);
                Navigation.NavigateTo("/stock-receipt-list");
            }
        }
        else if (confirmed && ShowDeleteDetailConfirm)
        {
            await StockOrderDetailService.DeleteAsync(EditStockOrderDetail.Id);
            EditStockOrder.DetailList.RemoveAll(dnd => dnd.Id == EditStockOrderDetail.Id);
        }

        ShowDeleteReceiptConfirm = false;
        ShowDeleteDetailConfirm = false;
    }

    protected void EditDetail(StockOrderDetailResponseModel activity)
    {
        EditStockOrderDetail = new StockOrderDetailEditModel
        {
            Id = activity.Id,
            StockOrderId = EditStockOrder.Id,
            ProductId = activity.ProductId,
            ProductTypeId = activity.ProductTypeId,
            Quantity = activity.Quantity,
            Deleted = activity.Deleted
        };
        ShowEditDetailForm = true;
    }

    //protected async Task DeleteDetail(int id)
    //{
    //    await StockOrderDetailService.DeleteAsync(id);
    //    EditStockOrder.DetailList.RemoveAll(dnd => dnd.Id == id);
    //}
    protected void DeleteDetail(int id)
    {
        EditStockOrderDetail = new StockOrderDetailEditModel { Id = id };
        SelectedItemName = EditStockOrder.DetailList.Where(p => p.Id == id).FirstOrDefault()?.ProductName ?? string.Empty;
        ShowDeleteDetailConfirm = true;
    }

}
