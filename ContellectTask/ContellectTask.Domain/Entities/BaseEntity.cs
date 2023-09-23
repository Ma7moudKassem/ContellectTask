namespace ContellectTask.Domain;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreationDateTime { get; set; }
    public string? CreatorUserName { get; set; }
    [NotMapped] public int Index { get; set; }

    public BaseEntity()
    {
        Id = Guid.NewGuid();
        CreationDateTime = DateTime.Now;
    }
}