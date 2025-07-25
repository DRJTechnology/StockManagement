using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;
using StockManagement.Models.Emuns;

[Authorize]
public partial class ContactsBase : ComponentBase
{
    [Inject]
    protected IContactDataService ContactService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected List<ContactResponseModel> Contacts { get; set; } = new();
    protected ContactEditModel EditContact { get; set; } = new();
    protected bool ShowForm { get; set; }
    protected bool ShowDeleteConfirm { get; set; } = false;
    protected string SelectedItemName { get; set; } = string.Empty;
    protected bool IsLoading { get; set; }
    protected bool ShowNotesPanel { get; set; } = false;
    protected ContactResponseModel? SelectedContact { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadContacts();
        }
    }

    protected async Task LoadContacts()
    {
        Contacts = (await ContactService.GetAllAsync())?.ToList() ?? new();
        IsLoading = false;
    }

    protected void ShowAddForm()
    {
        EditContact = new ContactEditModel();
        ShowForm = true;
    }

    protected void Edit(ContactResponseModel contact)
    {
        EditContact = new ContactEditModel
        {
            Id = contact.Id,
            Name = contact.Name,
            ContactTypeId = contact.ContactTypeId,
            Notes = contact.Notes
        };
        ShowForm = true;
    }

    protected async Task HandleValidSubmit()
    {
        if (EditContact.Id == 0)
        {
            var newId = await ContactService.CreateAsync(EditContact);
            Contacts.Add(
                new ContactResponseModel()
                {
                    Id = newId,
                    Name = EditContact.Name,
                    ContactTypeId = EditContact.ContactTypeId,
                    Type = ((ContactTypeEnum)EditContact.ContactTypeId).ToString(),
                    Notes = EditContact.Notes,
                    Deleted = false
                });
        }
        else
        {
            await ContactService.UpdateAsync(EditContact);
            var index = Contacts.FindIndex(v => v.Id == EditContact.Id);
            if (index >= 0)
            {
                Contacts[index] = new ContactResponseModel()
                {
                    Id = EditContact.Id,
                    Name = EditContact.Name,
                    ContactTypeId = EditContact.ContactTypeId,
                    Type = ((ContactTypeEnum)EditContact.ContactTypeId).ToString(),
                    Notes = EditContact.Notes,
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
        EditContact = new ContactEditModel { Id = id };
        SelectedItemName = Contacts.Where(p => p.Id == EditContact.Id).FirstOrDefault()?.Name ?? string.Empty;
        ShowDeleteConfirm = true;
    }

    protected async Task HandleDeleteConfirmation(bool confirmed)
    {
        ShowDeleteConfirm = false;
        if (confirmed)
        {
            await ContactService.DeleteAsync(EditContact.Id);
            Contacts.RemoveAll(p => p.Id == EditContact.Id);
            EditContact = new ContactEditModel();
        }
    }

    protected void ShowNotes(ContactResponseModel contact)
    {
        SelectedContact = contact;
        ShowNotesPanel = true;
    }

    protected void CloseNotesPanel()
    {
        ShowNotesPanel = false;
        SelectedContact = null;
    }
}
