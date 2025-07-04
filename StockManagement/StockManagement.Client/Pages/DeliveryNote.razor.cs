using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;
using System.ComponentModel.DataAnnotations;

public partial class DeliveryNoteBase : ComponentBase
{
    [Parameter] public int DeliveryNoteId { get; set; }

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

    protected bool IsLoading { get; set; }
    protected DeliveryNoteResponseModel EditDeliveryNote { get; set; } = new();
    protected DeliveryNoteDetailEditModel EditDeliveryNoteDetail { get; set; } = new();

    protected bool ShowEditDetailForm { get; set; }

    protected EditContext editContext;
    protected bool IsFormValid = false;

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadLookups();
            await LoadDeliveryNote();

            editContext = new EditContext(EditDeliveryNote);
            IsFormValid = editContext.Validate();
            editContext.OnFieldChanged += (_, __) =>
            {
                IsFormValid = editContext.Validate();
                InvokeAsync(StateHasChanged);
            };
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
    }

    protected async Task LoadDeliveryNote()
    {
        if (DeliveryNoteId == 0) {
            var localNow = await JavascriptMethodsService.GetLocalDateTimeAsync();
            EditDeliveryNote = new DeliveryNoteResponseModel() { Date = localNow };
        } else
        {
            EditDeliveryNote = await DeliveryNoteService.GetByIdAsync(DeliveryNoteId);
            if (EditDeliveryNote == null)
            {
                EditDeliveryNote = new DeliveryNoteResponseModel();
            }
        }
        IsLoading = false;
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

    protected async Task DeleteDetail(int id)
    {
        await DeliveryNoteDetailService.DeleteAsync(id);
        EditDeliveryNote.DetailList.RemoveAll(dnd => dnd.Id == id);
    }

    [Inject] IJSRuntime JS { get; set; }

    protected async Task DownloadPdf()
    {
        var url = "/api/pdf/delivery-note/1";
        await JS.InvokeVoidAsync("downloadFile", url);
    }

}
