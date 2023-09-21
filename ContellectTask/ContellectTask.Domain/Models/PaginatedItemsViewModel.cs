namespace ContellectTask.Domain;

public class PaginatedItemsViewModel<TEntity> where TEntity : BaseEntity
{
    public MetaData MetaData { get; set; }
    public PaginatedItemsViewModel() { }
    public PaginatedItemsViewModel(List<TEntity> items, int count, int pageNumber, int pageSize)
    {
        MetaData = new MetaData
        {
            TotalCount = count,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };
    }
}