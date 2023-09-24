namespace ContellectTask.WebUI.Shared;

public partial class MainLayout
{
    bool RigthToLift { get => _interopSettings.GetCurrentLanguage() == "ar-EG"; set { } }

    string Dir => RigthToLift ? "rtl" : "ltr";
    ToastsPlacement ToasterPalcement => RigthToLift ? ToastsPlacement.TopLeft : ToastsPlacement.TopRight;

    void ChangeLanguage()
    {
        Culture = !RigthToLift ? new CultureInfo("ar-EG") : new CultureInfo("en-US");

        StateHasChanged();
    }

    CultureInfo[] cultures = new[]
    {
            new CultureInfo("ar-EG"),
            new CultureInfo("en-US"),
    };

    CultureInfo Culture
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

    void LogOut() =>
        _navigationManager.NavigateTo("/logout");
}