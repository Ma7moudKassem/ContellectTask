namespace ContellectTask.Domain;

public class PaginatedItemsViewModel<TEntity> : List<TEntity> where TEntity : BaseEntity
{
    public MetaData MetaData { get; set; }
    public PaginatedItemsViewModel(IEnumerable<TEntity> items, long count, int pageNumber, int pageSize)
    {
        MetaData = new MetaData
        {
            TotalCount = count,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };

        AddRange(items);
    }
}