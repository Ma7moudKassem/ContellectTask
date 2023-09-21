﻿namespace ContellectTask.Infrastructure;

public class ContactRepository : IContactRepository
{
    private readonly ApplicationDbContext _context;
    public ContactRepository(ApplicationDbContext context) =>
        _context = context;

    public async Task<Contact?> GetContactAsync(Guid id) =>
        await _context.Contacts
                      .Include(x => x.ContactAddresses)
                      .ThenInclude(x => x.Address)
                      .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<Contact>> GetContactAsync(int pageSize = 5, int pageIndex = 0) =>
        await _context.Contacts
                      .Include(x => x.ContactAddresses)
                      .ThenInclude(x => x.Address)
                      .OrderBy(x => x.Name)
                      .Skip(pageSize * pageIndex)
                      .Take(pageSize)
                      .ToListAsync();

    public async Task AddContactAsync(Contact contact)
    {
        await _context.AddAsync(contact);
        await _context.SaveChangesAsync();
    }

    public async Task EditContactAsync(Contact contact)
    {
        _ = await GetContactAsync(contact.Id)
            ?? throw new ContactNotFoundException(contact.Id);

        _context.Update(contact);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteContactAsync(Guid id)
    {
        Contact? contact = await GetContactAsync(id)
            ?? throw new ContactNotFoundException(id);

        _context.Remove(contact);
        await _context.SaveChangesAsync();
    }
}