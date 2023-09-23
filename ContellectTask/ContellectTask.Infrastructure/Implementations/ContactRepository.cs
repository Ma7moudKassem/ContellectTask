namespace ContellectTask.Infrastructure;

public class ContactRepository : IContactRepository
{
    private readonly ApplicationDbContext _context;
    public ContactRepository(ApplicationDbContext context) =>
        _context = context;

    public async Task<Contact?> GetContactAsync(Guid id) =>
        await _context.Contacts.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<Contact>> GetContactAsync(int pageSize, int pageIndex) =>
        await _context.Contacts
                      .OrderBy(x => x.CreationTimeDate)
                      .Skip(pageSize * (pageIndex - 1))
                      .Take(pageSize)
                      .ToListAsync();

    public async Task AddContactAsync(Contact contact)
    {
        try
        {
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new ContactDomainException(ex.Message);
        }
    }

    public async Task EditContactAsync(Contact contact)
    {
        _ = await GetContactAsync(contact.Id)
            ?? throw new ContactNotFoundException(contact.Id);
        try
        {
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task DeleteContactAsync(Guid id)
    {
        Contact? contact = await GetContactAsync(id)
            ?? throw new ContactNotFoundException(id);

        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();
    }

    public async Task<long> CountAsync() =>
        await _context.Contacts.LongCountAsync();
}
