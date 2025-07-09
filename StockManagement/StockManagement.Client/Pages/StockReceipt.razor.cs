using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;

public partial class StockReceiptBase : ComponentBase
{
    [Parameter] public int StockReceiptId { get; set; }

    [Inject]
    IJSRuntime JS { get; set; }

    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    [Inject]
    protected IStockReceiptDataService StockReceiptService { get; set; } = default!;

    [Inject]
    protected IStockReceiptDetailDataService StockReceiptDetailService { get; set; } = default!;

    [Inject]
    protected ILookupsDataService LookupsService { get; set; } = default!;

    [Inject]
    protected IJavascriptMethodsService JavascriptMethodsService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected LookupsModel Lookups { get; private set; }

    protected bool IsLoading { get; set; }
    protected StockReceiptResponseModel EditStockReceipt { get; set; } = new();
    protected StockReceiptDetailEditModel EditStockReceiptDetail { get; set; } = new();

    protected bool ShowEditDetailForm { get; set; }

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
            await LoadStockReceipt();
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
        if (EditStockReceipt.Id == 0)
        {
            var newId = await StockReceiptService.CreateAsync(EditStockReceipt);
            EditStockReceipt.Id = newId;
        }
        else
        {
            await StockReceiptService.UpdateAsync(EditStockReceipt);
        }
        IsDirty = false;
    }

    protected async Task LoadStockReceipt()
    {
        if (StockReceiptId == 0)
        {
            var localNow = await JavascriptMethodsService.GetLocalDateTimeAsync();
            EditStockReceipt = new StockReceiptResponseModel() { Date = localNow, SupplierId = 0, DetailList = new List<StockReceiptDetailResponseModel>() };
        }
        else
        {
            EditStockReceipt = await StockReceiptService.GetByIdAsync(StockReceiptId);
            if (EditStockReceipt == null)
            {
                throw new Exception("Invalid Stock Receipt");
            }
        }
        CreateValidationContext();

        IsLoading = false;
    }

    private void CreateValidationContext()
    {
        editContext = new EditContext(EditStockReceipt);
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
        EditStockReceiptDetail = new StockReceiptDetailEditModel() { StockReceiptId = EditStockReceipt.Id };
        ShowEditDetailForm = true;
    }

    protected void CancelDetailEdit()
    {
        ShowEditDetailForm = false;
    }
    protected async Task HandleValidDetailSubmit()
    {
        if (EditStockReceiptDetail.Id == 0)
        {
            var newId = await StockReceiptDetailService.CreateAsync(EditStockReceiptDetail);
            EditStockReceipt.DetailList.Add(
                new StockReceiptDetailResponseModel()
                {
                    Id = newId,
                    StockReceiptId = EditStockReceipt.Id,
                    ProductId = EditStockReceiptDetail.ProductId,
                    ProductName = Lookups.ProductList.Where(p => p.Id == EditStockReceiptDetail.ProductId).FirstOrDefault()!.ProductName,
                    ProductTypeId = EditStockReceiptDetail.ProductTypeId,
                    ProductTypeName = Lookups.ProductTypeList.Where(pt => pt.Id == EditStockReceiptDetail.ProductTypeId).FirstOrDefault()!.ProductTypeName,
                    Quantity = EditStockReceiptDetail.Quantity,
                    Deleted = false
                });
        }
        else
        {
            await StockReceiptDetailService.UpdateAsync(EditStockReceiptDetail);
            var index = EditStockReceipt.DetailList.FindIndex(dnd => dnd.Id == EditStockReceiptDetail.Id);
            if (index >= 0)
            {
                EditStockReceipt.DetailList[index] = new StockReceiptDetailResponseModel()
                {
                    Id = EditStockReceiptDetail.Id,
                    StockReceiptId = EditStockReceipt.Id,
                    ProductId = EditStockReceiptDetail.ProductId,
                    ProductName = Lookups.ProductList.Where(p => p.Id == EditStockReceiptDetail.ProductId).FirstOrDefault()!.ProductName,
                    ProductTypeId = EditStockReceiptDetail.ProductTypeId,
                    ProductTypeName = Lookups.ProductTypeList.Where(pt => pt.Id == EditStockReceiptDetail.ProductTypeId).FirstOrDefault()!.ProductTypeName,
                    Quantity = EditStockReceiptDetail.Quantity,
                    Deleted = false
                };
            }
        }
        ShowEditDetailForm = false;
    }

    protected void DeleteStockReceipt()
    {
        if (EditStockReceipt.Id > 0)
        {
            StockReceiptService.DeleteAsync(EditStockReceipt.Id);
            Navigation.NavigateTo("/stock-receipt-list");
        }
    }

    protected void EditDetail(StockReceiptDetailResponseModel activity)
    {
        EditStockReceiptDetail = new StockReceiptDetailEditModel
        {
            Id = activity.Id,
            StockReceiptId = EditStockReceipt.Id,
            ProductId = activity.ProductId,
            ProductTypeId = activity.ProductTypeId,
            Quantity = activity.Quantity,
            Deleted = activity.Deleted
        };
        ShowEditDetailForm = true;
    }

    protected async Task DeleteDetail(int id)
    {
        await StockReceiptDetailService.DeleteAsync(id);
        EditStockReceipt.DetailList.RemoveAll(dnd => dnd.Id == id);
    }
}
