namespace ContellectTask.Domain;

public class Contact : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? Notes { get; set; }

    public Contact() { }
    public Contact(string name, string phone, string? notes, string address)
    {
        Name = name;
        Phone = phone;
        Notes = notes;
        Address = address;
    }
}