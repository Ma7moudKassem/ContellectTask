namespace ContellectTask.WebUI.Contacts;

public partial class ContactsPage
{
    bool showForm = false;
    bool isConnected = false;
    string currentUser = string.Empty;
    FeatureType featureType = FeatureType.Add;

    Contact contact = new();
    MetaData metaData = new();
    List<Contact> contacts = new();

    HubConnection? createContactHub;
    HubConnection? updateContactHub;
    HubConnection? deleteContactHub;

    protected override async Task OnInitializedAsync()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();

        currentUser = authState.User.Claims.First().Value;

        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var responce = await _contactsHttpInterceptor.GetContactsAsync();

            contacts = responce.Items ?? new();

            metaData = responce.MetaData ?? new();

            await SortContactsByIndex();
            await ConnectHubs();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    async Task OnContactObjectChange(Contact entity)
    {
        if (featureType.Equals(FeatureType.Add))
        {
            if (contacts.Count < 5)
                await OnPaginationChanged(metaData.CurrentPage);
            else
                await OnPaginationChanged(metaData.CurrentPage + 1);
        }
        if (featureType.Equals(FeatureType.Delete))
        {
            contacts.Remove(entity);

            if (contacts.Count == 0 && metaData.TotalPages > 1)
                await OnPaginationChanged(metaData.CurrentPage - 1);
        }
        if (featureType.Equals(FeatureType.Edit))
        {
            Contact oldContact = contacts.FirstOrDefault(x => x.Id == entity.Id) ?? new();

            contacts.Remove(oldContact);
            contacts.Add(entity);
        }

        await SortContactsByIndex();

        string message = JsonConvert.SerializeObject(contact);

        await SendAsync(message);
    }

    async Task OnCancel()
    {
        showForm = false;

        await InvokeAsync(StateHasChanged);
    }

    async Task ShowForm(FeatureType featureType, Contact entity)
    {
        this.featureType = featureType;

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

    async Task OnPaginationChanged(int pageIndex)
    {
        var pagingResponse = await _contactsHttpInterceptor.GetContactsAsync(pageIndex: pageIndex);

        contacts = pagingResponse.Items ?? new();
        metaData = pagingResponse.MetaData ?? new();

        await SortContactsByIndex();
    }

    #region HubMethods
    async Task ConnectHubs()
    {
        createContactHub = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri("/CreateContactHub")).Build();

        updateContactHub = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri("/UpdateContactHub")).Build();

        deleteContactHub = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri("/DeleteContactHub")).Build();

        createContactHub.On<string>("ReceiveMessage", AddContactFromHub);
        updateContactHub.On<string>("ReceiveMessage", EditContactFromHub);
        deleteContactHub.On<string>("ReceiveMessage", DeleteContactFromHub);

        await createContactHub.StartAsync();
        await updateContactHub.StartAsync();
        await deleteContactHub.StartAsync();

        isConnected = true;
    }

    async Task SendAsync(string message)
    {
        if (featureType.Equals(FeatureType.Add))
        {
            if (createContactHub is not null && isConnected && !string.IsNullOrWhiteSpace(message))
                await createContactHub.SendAsync("SendMessage", message);
        }

        if (featureType.Equals(FeatureType.Edit))
        {
            if (updateContactHub is not null && isConnected && !string.IsNullOrWhiteSpace(message))
                await updateContactHub.SendAsync("SendMessage", message);
        }

        if (featureType.Equals(FeatureType.Delete))
        {
            if (deleteContactHub is not null && isConnected && !string.IsNullOrWhiteSpace(message))
                await deleteContactHub.SendAsync("SendMessage", message);
        }
    }

    async Task AddContactFromHub(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            if (contacts.Count <= 5)
                await OnPaginationChanged(metaData.CurrentPage);
            else
                await OnPaginationChanged(metaData.CurrentPage + 1);
        }
    }

    async Task EditContactFromHub(string message)
    {
        if (!string.IsNullOrEmpty(message))
            await OnPaginationChanged(metaData.CurrentPage);
    }

    async Task DeleteContactFromHub(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            if (contacts.Count == 0 && metaData.TotalPages > 1)
                await OnPaginationChanged(metaData.CurrentPage - 1);
            else
                await OnPaginationChanged(metaData.CurrentPage);
        }
    }
    #endregion
}