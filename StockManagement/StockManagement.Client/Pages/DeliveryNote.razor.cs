using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;

public partial class DeliveryNoteBase : ComponentBase
{
    [Parameter] public int DeliveryNoteId { get; set; }

    [Inject]
    IJSRuntime JS { get; set; }

    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    [Inject]
    protected IDeliveryNoteDataService DeliveryNoteService { get; set; } = default!;

    [Inject]
    protected IDeliveryNoteDetailDataService DeliveryNoteDetailService { get; set; } = default!;

    [Inject]
    protected ILookupsDataService LookupsService { get; set; } = default!;

    [Inject]
    protected IJavascriptMethodsService JavascriptMethodsService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected LookupsModel Lookups { get; private set; }
    protected bool ShowDeleteNoteConfirm { get; set; } = false;
    protected bool ShowDeleteDetailConfirm { get; set; } = false;
    protected string SelectedItemName { get; set; } = string.Empty;

    protected bool IsLoading { get; set; }
    protected DeliveryNoteResponseModel EditDeliveryNote { get; set; } = new();
    protected DeliveryNoteDetailEditModel EditDeliveryNoteDetail { get; set; } = new();

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
            await LoadDeliveryNote();
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
        if (EditDeliveryNote.Id == 0)
        {
            var newId = await DeliveryNoteService.CreateAsync(EditDeliveryNote);
            EditDeliveryNote.Id = newId;
        }
        else
        {
            await DeliveryNoteService.UpdateAsync(EditDeliveryNote);
        }
        IsDirty = false;
    }

    protected async Task LoadDeliveryNote()
    {
        if (DeliveryNoteId == 0)
        {
            var localNow = await JavascriptMethodsService.GetLocalDateTimeAsync();
            EditDeliveryNote = new DeliveryNoteResponseModel() { Date = localNow, LocationId = 0, DetailList = new List<DeliveryNoteDetailResponseModel>() };
        }
        else
        {
            EditDeliveryNote = await DeliveryNoteService.GetByIdAsync(DeliveryNoteId);
            if (EditDeliveryNote == null)
            {
                throw new Exception("Invalid Delivery Note");
            }
        }
        CreateValidationContext();

        IsLoading = false;
    }

    private void CreateValidationContext()
    {
        editContext = new EditContext(EditDeliveryNote);
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
        EditDeliveryNoteDetail = new DeliveryNoteDetailEditModel() { DeliveryNoteId = EditDeliveryNote.Id };
        ShowEditDetailForm = true;
    }

    protected void CancelDetailEdit()
    {
        ShowEditDetailForm = false;
    }
    protected async Task HandleValidDetailSubmit()
    {
        if (EditDeliveryNoteDetail.Id == 0)
        {
            var newId = await DeliveryNoteDetailService.CreateAsync(EditDeliveryNoteDetail);
            EditDeliveryNote.DetailList.Add(
                new DeliveryNoteDetailResponseModel()
                {
                    Id = newId,
                    DeliveryNoteId = EditDeliveryNote.Id,
                    ProductId = EditDeliveryNoteDetail.ProductId,
                    ProductName = Lookups.ProductList.Where(p => p.Id == EditDeliveryNoteDetail.ProductId).FirstOrDefault()!.ProductName,
                    ProductTypeId = EditDeliveryNoteDetail.ProductTypeId,
                    ProductTypeName = Lookups.ProductTypeList.Where(pt => pt.Id == EditDeliveryNoteDetail.ProductTypeId).FirstOrDefault()!.ProductTypeName,
                    Quantity = EditDeliveryNoteDetail.Quantity,
                    Deleted = false
                });
        }
        else
        {
            await DeliveryNoteDetailService.UpdateAsync(EditDeliveryNoteDetail);
            var index = EditDeliveryNote.DetailList.FindIndex(dnd => dnd.Id == EditDeliveryNoteDetail.Id);
            if (index >= 0)
            {
                EditDeliveryNote.DetailList[index] = new DeliveryNoteDetailResponseModel()
                {
                    Id = EditDeliveryNoteDetail.Id,
                    DeliveryNoteId = EditDeliveryNote.Id,
                    ProductId = EditDeliveryNoteDetail.ProductId,
                    ProductName = Lookups.ProductList.Where(p => p.Id == EditDeliveryNoteDetail.ProductId).FirstOrDefault()!.ProductName,
                    ProductTypeId = EditDeliveryNoteDetail.ProductTypeId,
                    ProductTypeName = Lookups.ProductTypeList.Where(pt => pt.Id == EditDeliveryNoteDetail.ProductTypeId).FirstOrDefault()!.ProductTypeName,
                    Quantity = EditDeliveryNoteDetail.Quantity,
                    Deleted = false
                };
            }
        }
        ShowEditDetailForm = false;
    }

    protected void DeleteDeliveryNote()
    {
        if (EditDeliveryNote.Id > 0)
        {
            SelectedItemName = "this entire Delivery Note";
            ShowDeleteNoteConfirm = true;
        }
    }

    protected async Task HandleDeleteConfirmation(bool confirmed)
    {
        if (confirmed && ShowDeleteNoteConfirm)
        {
            if (confirmed)
            {
                await DeliveryNoteService.DeleteAsync(EditDeliveryNote.Id);
                Navigation.NavigateTo("/delivery-note-list");
            }
        }
        else if (confirmed && ShowDeleteDetailConfirm)
        {
            await DeliveryNoteDetailService.DeleteAsync(EditDeliveryNoteDetail.Id);
            EditDeliveryNote.DetailList.RemoveAll(dnd => dnd.Id == EditDeliveryNoteDetail.Id);
        }

        ShowDeleteNoteConfirm = false;
        ShowDeleteDetailConfirm = false;
    }

    protected void EditDetail(DeliveryNoteDetailResponseModel activity)
    {
        EditDeliveryNoteDetail = new DeliveryNoteDetailEditModel
        {
            Id = activity.Id,
            DeliveryNoteId = EditDeliveryNote.Id,
            ProductId = activity.ProductId,
            ProductTypeId = activity.ProductTypeId,
            Quantity = activity.Quantity,
            Deleted = activity.Deleted
        };
        ShowEditDetailForm = true;
    }

    protected void DeleteDetail(int id)
    {
        EditDeliveryNoteDetail = new DeliveryNoteDetailEditModel { Id = id };
        SelectedItemName = EditDeliveryNote.DetailList.Where(p => p.Id == id).FirstOrDefault()?.ProductName ?? string.Empty;
        ShowDeleteDetailConfirm = true;
    }

    protected async Task DownloadDeliveryNotePdf()
    {
        var url = $"/api/pdf/delivery-note/{EditDeliveryNote.Id}";
        await JS.InvokeVoidAsync("open", url, "_blank");
    }

    protected async Task ConfirmDelivery()
    {
        EditDeliveryNote.DeliveryCompleted = true;
        await HandleValidSubmit();
    }
    protected bool DetailsExist
    {
        get => EditDeliveryNote != null && EditDeliveryNote.DetailList != null && EditDeliveryNote.DetailList.Count > 0;
    }

}
