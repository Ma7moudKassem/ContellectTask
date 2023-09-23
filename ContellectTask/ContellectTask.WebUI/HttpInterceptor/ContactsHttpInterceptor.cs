namespace ContellectTask.WebUI.HttpInterceptor;

public class ContactsHttpInterceptor : IContactsHttpInterceptor
{
    readonly Toaster _toaster;
    readonly HttpClient _httpClient;
    readonly ILocalStorageService _localStorage;
    readonly IStringLocalizer<Resource> _localizer;

    string baseUri = "api/v1/contacts";
    public ContactsHttpInterceptor(HttpClient httpClient, Toaster toaster, IStringLocalizer<Resource> localizer, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _toaster = toaster;
        _localizer = localizer;
        _localStorage = localStorage;
    }

    public async Task<PaginatedItemsResponceViewModel<Contact>> GetContactsAsync(int pageSize = 5, int pageIndex = 1)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"{baseUri}?pageSize={pageSize}&pageIndex={pageIndex}");

        string token = await GetTokenAsync();

        request.Headers.Add("Authorization", $"Bearer {token}");

        var response = await _httpClient.SendAsync(request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _toaster.ShowMessage(ToastType.Danger, content);
            throw new ApplicationException(content);
        }

        var pagingResponse = new PaginatedItemsResponceViewModel<Contact>
        {
            Items = JsonConvert.DeserializeObject<List<Contact>>(content),
            MetaData = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("X-Pagination").First())
        };

        return pagingResponse;
    }

    public async Task PostContactAsync(Contact contact)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, baseUri);

        string token = await GetTokenAsync();

        request.Headers.Add("Authorization", $"Bearer {token}");

        request.Content = new StringContent(JsonConvert.SerializeObject(contact), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _toaster.ShowMessage(ToastType.Danger, _localizer[Resource.FailedAddMessage]);
            throw new ApplicationException(content);
        }

        _toaster.ShowMessage(ToastType.Success, _localizer[Resource.SuccessfullAddMessage]);
    }

    public async Task EditContactAsync(Contact contact)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, baseUri);

        string token = await GetTokenAsync();

        request.Headers.Add("Authorization", $"Bearer {token}");

        request.Content = new StringContent(JsonConvert.SerializeObject(contact), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _toaster.ShowMessage(ToastType.Danger, _localizer[Resource.FailedEditMessage]);
            throw new ApplicationException(content);
        }

        _toaster.ShowMessage(ToastType.Success, _localizer[Resource.SuccessfullEditMessage]);
    }

    public async Task DeleteContactAsync(Guid id)
    {

        var request = new HttpRequestMessage(HttpMethod.Delete, $"{baseUri}/{id}");

        string token = await GetTokenAsync();

        request.Headers.Add("Authorization", $"Bearer {token}");

        var response = await _httpClient.SendAsync(request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _toaster.ShowMessage(ToastType.Danger, _localizer[Resource.FailedDeleteMessage]);
            throw new ApplicationException(content);
        }

        _toaster.ShowMessage(ToastType.Success, _localizer[Resource.SuccessfullDeleteMessage]);
    }

    async Task<string> GetTokenAsync() =>
        await _localStorage.GetItemAsync<string>("authToken");

}