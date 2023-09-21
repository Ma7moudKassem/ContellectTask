namespace ContellectTask.WebUI.Contacts;

public partial class ContactsForm
{
    [Parameter] public Contact Contact { get; set; } = null!;
    [Parameter] public FeatureType FeatureType { get; set; }
    [Parameter] public EventCallback<Contact> OnContactObjectChange { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public bool ShowForm { get; set; }

    bool disabledInput => FeatureType.Equals(FeatureType.Details) || FeatureType.Equals(FeatureType.Delete);
    async Task OnValidSubmit()
    {
        if (FeatureType.Equals(FeatureType.Add))
            await _httpClient.PostAsJsonAsync("api/v1/Contacts", Contact);
        if (FeatureType.Equals(FeatureType.Edit))
            await _httpClient.PutAsJsonAsync("api/v1/Contacts", Contact);
        if (FeatureType.Equals(FeatureType.Delete))
            await _httpClient.DeleteAsync($"api/v1/Contacts/{Contact.Id}");

        await OnContactObjectChange.InvokeAsync(Contact);
        await OnCancel.InvokeAsync(Contact);
    }

    async Task CloseForm() =>
        await OnCancel.InvokeAsync();
}