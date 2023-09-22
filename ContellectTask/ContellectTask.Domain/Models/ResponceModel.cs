namespace ContellectTask.Domain;

public class ResponceModel
{
    public string? Code { get; set; }
    public string? Status { get; set; }
    public string? Message { get; set; }

    public DateTime LastModification { get; set; } = DateTime.Now;
}
