namespace ContellectTask.Domain;

public class PaginatedItemsResponceViewModel<TEntity> where TEntity : class
{
    public List<TEntity>? Items { get; set; }
    public MetaData? MetaData { get; set; }
}
