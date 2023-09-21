namespace ContellectTask.WebUI.Shared;

public partial class MainLayout
{
    [Inject] public IJSRuntime _jsRuntime { get; set; } = null!;
    [Inject] public NavigationManager _navigationManager { get; set; } = null!;

    public bool RigthToLift { get => _interopSettings.GetCurrentLanguage() == "ar-EG"; set { } }
    public string Dir => RigthToLift ? "rtl" : "ltr";

    public void ChangeLanguage()
    {
        Culture = !RigthToLift ? new CultureInfo("ar-EG") : new CultureInfo("en-US");

        StateHasChanged();
    }

    private CultureInfo[] cultures = new[]
    {
            new CultureInfo("ar-EG"),
            new CultureInfo("en-US"),
    };
    private CultureInfo Culture
    {
        get => CultureInfo.CurrentCulture;
        set
        {
            if (CultureInfo.CurrentCulture != value)
            {
                var js = (IJSInProcessRuntime)_jsRuntime;
                js.InvokeVoid("blazorCulture.set", value.Name);

                _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
            }
        }
    }
}
