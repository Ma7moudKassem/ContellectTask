namespace ContellectTask.WebUI.Helper;

public class Toaster
{
    readonly ToastService _toastService;
    readonly IStringLocalizer<Resource> _localizer;
    public Toaster(IStringLocalizer<Resource> localizer, ToastService toastService)
    {
        _localizer = localizer;
        _toastService = toastService;
    }

    public void ShowMessage(ToastType toastType, string message) =>
       _toastService.Notify(CreateToastMessage(toastType, message));

    private ToastMessage CreateToastMessage(ToastType toastType, string message)
        => new()
        {
            Type = toastType,
            Title = _localizer[Resource.ContactsApplication],
            HelpText = $"{DateTime.Now}",
            Message = $"{message}",
            AutoHide = true,
        };
}