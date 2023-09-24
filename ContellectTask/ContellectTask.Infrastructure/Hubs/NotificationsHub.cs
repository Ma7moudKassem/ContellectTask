namespace ContellectTask.Infrastructure;

public class NotificationsHub : Hub
{
    public async Task SendMessage(string message) =>
        await Clients.All.SendAsync("ReceiveMessage", message);

    public override async Task OnConnectedAsync() =>
        await base.OnConnectedAsync();

    public override async Task OnDisconnectedAsync(Exception? e) =>
        await base.OnDisconnectedAsync(e);
}