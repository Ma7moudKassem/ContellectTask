namespace ContellectTask.WebUI.Helper;

public class InteropSettings
{
    readonly IJSRuntime _jSRuntime;
    public InteropSettings(IJSRuntime jSRuntime)
        => _jSRuntime = jSRuntime;

    public string GetCurrentLanguage()
    {
        IJSInProcessRuntime js = (IJSInProcessRuntime)_jSRuntime;

        string currentLanguage = js.Invoke<string>("blazorCulture.get");

        return currentLanguage;
    }

    public void SetLanguageToLocalStorage(string value)
    {
        IJSInProcessRuntime js = (IJSInProcessRuntime)_jSRuntime;

        js.InvokeVoid("blazorCulture.set", value);
    }
}
