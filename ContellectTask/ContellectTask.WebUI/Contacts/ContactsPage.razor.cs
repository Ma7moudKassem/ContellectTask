namespace ContellectTask.WebUI.Contacts;

public partial class ContactsPage
{
    bool showForm = false;
    FeatureType featureType = FeatureType.Add;

    Contact contact = new();

    PaginatedItemsViewModel<Contact> paginated = new();

    List<Contact> contacts = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            paginated = await _httpClient.GetFromJsonAsync<PaginatedItemsViewModel<Contact>>("api/v1/contacts?pageSize=5&pageIndex=0") ?? new();

            //contacts = paginated.MetaData.ToList();

            await SortContactsByIndex();
            await base.OnAfterRenderAsync(firstRender);
        }
    }

    async Task OnContactObjectChange(Contact entity)
    {
        if (featureType.Equals(FeatureType.Add))
        {
            if (contacts.Count < 5)
                contacts.Add(entity);
        }
        if (featureType.Equals(FeatureType.Delete))
            contacts.Remove(entity);
        if (featureType.Equals(FeatureType.Edit))
        {
            Contact oldContact = contacts.FirstOrDefault(x => x.Id == entity.Id) ?? new();
            contacts.Remove(oldContact);
            contacts.Add(entity);
        }

        await SortContactsByIndex();
    }

    async Task OnCancel()
    {
        showForm = false;

        await InvokeAsync(StateHasChanged);
    }

    async Task ShowForm(FeatureType formFeatureType, Contact entity)
    {
        featureType = formFeatureType;
        contact = entity;

        showForm = true;

        await InvokeAsync(StateHasChanged);
    }

    async Task SortContactsByIndex()
    {
        await Task.Run(() =>
        {
            int i = 1;
            contacts.ForEach(x => x.Index = i++);
        });

        await InvokeAsync(StateHasChanged);
    }

    async Task OnPaginationChange(int pageIndex)
    {
        paginated = await _httpClient.GetFromJsonAsync<PaginatedItemsViewModel<Contact>>($"api/v1/contacts?pageSize=5&pageIndex={pageIndex}") ?? new();

        //contacts = paginated.Data.ToList();

        await SortContactsByIndex();
    }
}