namespace ContellectTask.WebUI.logIn;

public partial class LogInForm
{
    public bool ShowAuthError { get; set; }

    AuthModel authModel = new();
    LogInModel logInModel = new();

    public async Task ExecuteLogin()
    {
        ShowAuthError = false;

        HttpResponseMessage result = await _httpClient.PostAsJsonAsync("api/v1/auth/login", logInModel);

        if (result.IsSuccessStatusCode)
            authModel = await result.Content.ReadFromJsonAsync<AuthModel>() ?? new();

        if (!authModel.IsAuthenticated)
        {
            ShowAuthError = true;
            _toaster.ShowMessage(ToastType.Danger, Resource.ErrorEmailOrPassword);
        }
        else
        {
            await _localStorage.SetItemAsync("authToken", authModel.Token);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authModel.Token);

            _navigationManager.NavigateTo("/");
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(authModel.UserName);
            _toaster.ShowMessage(ToastType.Success, Resource.LogInSuccessfully);
        }
    }
}