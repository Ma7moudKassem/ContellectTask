namespace ContellectTask.Domain;

public class ContactValidator : AbstractValidator<Contact>
{
    public ContactValidator(IStringLocalizer<Resource> localizer)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Resource.Requierd]);

        RuleFor(x => x.Phone).NotEmpty().WithMessage(localizer[Resource.Requierd]);
        RuleFor(x => x.Phone).Matches(@"^([\+]?20)?[1-9][0-9]{9}$")
            .WithMessage(localizer[Resource.PhoneFormat]);

        RuleFor(x => x.Address).NotEmpty().WithMessage(localizer[Resource.Requierd]);
    }
}