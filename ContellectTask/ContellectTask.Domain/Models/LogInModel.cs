namespace ContellectTask.Domain;

public class LogInModel
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; }
}