using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Client.Pages;
using StockManagement.Models;

[Authorize]
public partial class VenuesBase : ComponentBase
{
    [Inject]
    protected IVenueDataService VenueService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected List<VenueResponseModel> Venues { get; set; } = new();
    protected VenueEditModel EditVenue { get; set; } = new();
    protected bool ShowForm { get; set; }
    protected bool ShowDeleteConfirm { get; set; } = false;
    protected string SelectedItemName { get; set; } = string.Empty;
    protected bool IsLoading { get; set; }
    protected bool ShowNotesPanel { get; set; } = false;
    protected VenueResponseModel? SelectedVenue { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadVenues();
        }
    }

    protected async Task LoadVenues()
    {
        Venues = (await VenueService.GetAllAsync())?.ToList() ?? new();
        IsLoading = false;
    }

    protected void ShowAddForm()
    {
        EditVenue = new VenueEditModel();
        ShowForm = true;
    }

    protected void Edit(VenueResponseModel venue)
    {
        EditVenue = new VenueEditModel
        {
            Id = venue.Id,
            VenueName = venue.VenueName,
            Notes = venue.Notes
        };
        ShowForm = true;
    }

    protected async Task HandleValidSubmit()
    {
        if (EditVenue.Id == 0)
        {
            var newId = await VenueService.CreateAsync(EditVenue);
            Venues.Add(
                new VenueResponseModel()
                {
                    Id = newId,
                    VenueName = EditVenue.VenueName,
                    Notes = EditVenue.Notes,
                    Deleted = false
                });
        }
        else
        {
            await VenueService.UpdateAsync(EditVenue);
            var index = Venues.FindIndex(v => v.Id == EditVenue.Id);
            if (index >= 0)
            {
                Venues[index] = new VenueResponseModel()
                {
                    Id = EditVenue.Id,
                    VenueName = EditVenue.VenueName,
                    Notes = EditVenue.Notes,
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
        EditVenue = new VenueEditModel { Id = id };
        SelectedItemName = Venues.Where(p => p.Id == EditVenue.Id).FirstOrDefault()?.VenueName ?? string.Empty;
        ShowDeleteConfirm = true;
    }

    protected async Task HandleDeleteConfirmation(bool confirmed)
    {
        ShowDeleteConfirm = false;
        if (confirmed)
        {
            await VenueService.DeleteAsync(EditVenue.Id);
            Venues.RemoveAll(p => p.Id == EditVenue.Id);
            EditVenue = new VenueEditModel();
        }
    }

    protected void ShowNotes(VenueResponseModel venue)
    {
        SelectedVenue = venue;
        ShowNotesPanel = true;
    }

    protected void CloseNotesPanel()
    {
        ShowNotesPanel = false;
        SelectedVenue = null;
    }
}
