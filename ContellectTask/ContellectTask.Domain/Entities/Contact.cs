namespace ContellectTask.Domain;

public class Contact : BaseEntity
{
    public Contact() { }
    public Contact(string name, string phone, string? notes)
    {
        Name = name;
        Phone = phone;
        Notes = notes;
    }

    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Notes { get; set; }

    public List<ContactAddress> ContactAddresses { get; set; } = new();

    public void AddContactAddress(ContactAddress contactAddress)
    {
        if (contactAddress.Address is null)
            throw new ContactDomainException("Address is requierd");

        if (string.IsNullOrEmpty(contactAddress.Address.Country))
            throw new ContactDomainException("Country is requierd");

        if (string.IsNullOrEmpty(contactAddress.Address.City))
            throw new ContactDomainException("City is requierd");

        if (string.IsNullOrEmpty(contactAddress.Address.Street))
            throw new ContactDomainException("Street is requierd");

        ContactAddresses.Add(contactAddress);
    }

    public void DeleteContactAddress(ContactAddress contactAddress)
    {
        if (ContactAddresses.Any())
        {
            ContactAddresses.Remove(contactAddress);
            return;
        }
    }
}