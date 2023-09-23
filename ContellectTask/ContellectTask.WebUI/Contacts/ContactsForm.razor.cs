namespace ContellectTask.WebUI.Contacts;

public partial class ContactsForm
{
    [Parameter] public Contact Contact { get; set; } = null!;
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback<Contact> OnContactObjectChange { get; set; }
    [Parameter] public bool ShowForm { get; set; }
    [Parameter] public FeatureType FeatureType { get; set; }

    bool DisabledInput { get => FeatureType.Equals(FeatureType.Details) || FeatureType.Equals(FeatureType.Delete); set { } }

    async Task OnValidSubmit()
    {
        if (FeatureType.Equals(FeatureType.Add))
            await _contactsHttpInterceptor.PostContactAsync(Contact);
        if (FeatureType.Equals(FeatureType.Edit))
            await _contactsHttpInterceptor.EditContactAsync(Contact);
        if (FeatureType.Equals(FeatureType.Delete))
            await _contactsHttpInterceptor.DeleteContactAsync(Contact.Id);

        await OnContactObjectChange.InvokeAsync(Contact);
        await OnCancel.InvokeAsync(Contact);
    }

    async Task CloseForm() =>
        await OnCancel.InvokeAsync();
}