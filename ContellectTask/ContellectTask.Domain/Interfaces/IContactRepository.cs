namespace ContellectTask.Domain;

public interface IContactRepository
{
    Task<long> CountAsync();
    Task<Contact?> GetContactAsync(Guid id);
    Task<IEnumerable<Contact>> GetContactAsync(int pageSize = 5, int pageIndex = 0);
    Task AddContactAsync(Contact contact);
    Task EditContactAsync(Contact contact);
    Task DeleteContactAsync(Guid id);
}