namespace ContellectTask.Domain;

public sealed class ContactDomainException : Exception
{
    public ContactDomainException(string message) : base(message) { }
}
