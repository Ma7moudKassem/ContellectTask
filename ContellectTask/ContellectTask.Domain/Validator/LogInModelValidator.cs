namespace ContellectTask.Domain;

public class LogInModelValidator : AbstractValidator<LogInModel>
{
    public LogInModelValidator(IStringLocalizer<Resource> localizer)
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage(localizer[Resource.UserNameRequierd]);
        RuleFor(x => x.Password).NotEmpty().WithMessage(localizer[Resource.PasswordRequierd]);
    }
}