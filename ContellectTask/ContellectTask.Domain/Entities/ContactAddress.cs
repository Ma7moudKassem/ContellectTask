namespace ContellectTask.Domain;

public class ContactAddress : BaseEntity
{
    public Guid ContactId { get; set; }

    public Guid AddressId { get; set; }
    public Address? Address { get; set; }
}