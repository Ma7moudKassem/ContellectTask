namespace ContellectTask.Domain;

public sealed class ContactNotFoundException : Exception
{
    public ContactNotFoundException(Guid contactId) : base($"Contact with id: {contactId} is not found")
    { }
}