namespace ContellectTask.WebUI;

public static class WebAssemblyHostExtension
{
    public async static Task SetDefaultCulture(this WebAssemblyHost host)
    {
        CultureInfo culture;
        var js = host.Services.GetRequiredService<IJSRuntime>();
        var result = await js.InvokeAsync<string>("blazorCulture.get");

        if (result is not null)
            culture = new CultureInfo(result);
        else
        {
            culture = new CultureInfo("ar-EG");
            await js.InvokeVoidAsync("blazorCulture.set", "ar-EG");
        }

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}
