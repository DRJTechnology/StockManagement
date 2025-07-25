using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Client.Pages;
using StockManagement.Models;

[Authorize]
public partial class LocationsBase : ComponentBase
{
    [Inject]
    protected ILocationDataService LocationService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected List<LocationResponseModel> Locations { get; set; } = new();
    protected LocationEditModel EditLocation { get; set; } = new();
    protected bool ShowForm { get; set; }
    protected bool ShowDeleteConfirm { get; set; } = false;
    protected string SelectedItemName { get; set; } = string.Empty;
    protected bool IsLoading { get; set; }
    protected bool ShowNotesPanel { get; set; } = false;
    protected LocationResponseModel? SelectedLocation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadLocations();
        }
    }

    protected async Task LoadLocations()
    {
        Locations = (await LocationService.GetAllAsync())?.ToList() ?? new();
        IsLoading = false;
    }

    protected void ShowAddForm()
    {
        EditLocation = new LocationEditModel();
        ShowForm = true;
    }

    protected void Edit(LocationResponseModel location)
    {
        EditLocation = new LocationEditModel
        {
            Id = location.Id,
            Name = location.Name,
            Notes = location.Notes
        };
        ShowForm = true;
    }

    protected async Task HandleValidSubmit()
    {
        if (EditLocation.Id == 0)
        {
            var newId = await LocationService.CreateAsync(EditLocation);
            Locations.Add(
                new LocationResponseModel()
                {
                    Id = newId,
                    Name = EditLocation.Name,
                    Notes = EditLocation.Notes,
                    Deleted = false
                });
        }
        else
        {
            await LocationService.UpdateAsync(EditLocation);
            var index = Locations.FindIndex(v => v.Id == EditLocation.Id);
            if (index >= 0)
            {
                Locations[index] = new LocationResponseModel()
                {
                    Id = EditLocation.Id,
                    Name = EditLocation.Name,
                    Notes = EditLocation.Notes,
                    Deleted = false
                };
            }
        }
        ShowForm = false;
    }

    protected void CancelEdit()
    {
        ShowForm = false;
    }

    protected void Delete(int id)
    {
        EditLocation = new LocationEditModel { Id = id };
        SelectedItemName = Locations.Where(p => p.Id == EditLocation.Id).FirstOrDefault()?.Name ?? string.Empty;
        ShowDeleteConfirm = true;
    }

    protected async Task HandleDeleteConfirmation(bool confirmed)
    {
        ShowDeleteConfirm = false;
        if (confirmed)
        {
            await LocationService.DeleteAsync(EditLocation.Id);
            Locations.RemoveAll(p => p.Id == EditLocation.Id);
            EditLocation = new LocationEditModel();
        }
    }

    protected void ShowNotes(LocationResponseModel location)
    {
        SelectedLocation = location;
        ShowNotesPanel = true;
    }

    protected void CloseNotesPanel()
    {
        ShowNotesPanel = false;
        SelectedLocation = null;
    }
}
