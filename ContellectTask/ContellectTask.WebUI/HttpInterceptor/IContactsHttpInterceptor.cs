namespace ContellectTask.WebUI;

public interface IContactsHttpInterceptor
{
    Task<PaginatedItemsResponceViewModel<Contact>> GetContactsAsync(int pageSize = 5, int pageIndex = 1);
    Task PostContactAsync(Contact contact);
    Task EditContactAsync(Contact contact);
    Task DeleteContactAsync(Guid id);
}