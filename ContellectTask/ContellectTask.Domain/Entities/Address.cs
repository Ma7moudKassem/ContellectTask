namespace ContellectTask.Domain;

public class Address : BaseEntity
{
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string? MoreInfo { get; set; }
}