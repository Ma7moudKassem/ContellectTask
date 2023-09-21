namespace ContellectTask.Domain;

public class ContactValidator : AbstractValidator<Contact>
{
    public ContactValidator(IStringLocalizer<Resource> localizer)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(localizer[Resource.Requierd]);
        RuleFor(x => x.Phone).NotEmpty().WithMessage(localizer[Resource.Requierd]);
        RuleFor(x => x.Address).NotEmpty().WithMessage(localizer[Resource.Requierd]);
    }
}