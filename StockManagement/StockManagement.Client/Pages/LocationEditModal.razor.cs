using Microsoft.AspNetCore.Components;
using StockManagement.Models;

public partial class LocationEditModalBase : ComponentBase
{
    [Parameter] public bool Show { get; set; }
    [Parameter] public LocationEditModel EditLocation { get; set; } = new();
    [Parameter] public List<ContactResponseModel> Customers { get; set; } = new();
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback OnValidSubmit { get; set; }

    protected async Task HandleValidSubmit()
    {
        await OnValidSubmit.InvokeAsync();
    }
}
