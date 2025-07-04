using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;

[Authorize]
public partial class DeliveryNoteListBase : ComponentBase
{
    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    [Inject]
    protected IDeliveryNoteDataService DeliveryNoteService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected List<DeliveryNoteResponseModel> DeliveryNotes { get; set; } = new();
    protected bool IsLoading { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadDeliveryNotes();
        }
    }

    protected async Task LoadDeliveryNotes()
    {
        DeliveryNotes = (await DeliveryNoteService.GetAllAsync())?.ToList() ?? new();
        IsLoading = false;
    }

    protected void AddDeliveryNote()
    {
        Navigation.NavigateTo("/delivery-note/0");
    }

    protected void ViewDetails(DeliveryNoteResponseModel note)
    {
        Navigation.NavigateTo($"/delivery-note/{note.Id}");
    }

}
