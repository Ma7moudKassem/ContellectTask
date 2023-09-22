namespace ContellectTask.Domain;

public interface IAuthService
{
    Task<AuthModel> LogIn(LogInModel model);
}