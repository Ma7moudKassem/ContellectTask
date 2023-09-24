namespace ContellectTask.Domain;

public class AuthModel
{
    public string Token { get; set; } = null!;
    public string? Message { get; set; }
    public string UserName { get; set; } = null!;
    public bool IsAuthenticated { get; set; }
    public DateTime ExpiresOn { get; set; }
}