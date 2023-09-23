namespace ContellectTask.Api;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class ContactsController : ControllerBase
{
    private readonly IContactRepository _contactRepository;
    private readonly ILogger<ContactsController> _logger;
    public ContactsController(IContactRepository contactRepository, ILogger<ContactsController> logger)
    {
        _contactRepository = contactRepository;
        _logger = logger;
    }

    #region Get EndPoints
    //GET api/v1/contacts?pageSize=0&pageIndex=5
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedItemsViewModel<Contact>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllAsync([FromQuery] int pageSize = 5, [FromQuery] int pageIndex = 1)
    {
        long count = await _contactRepository.CountAsync();

        IEnumerable<Contact> contacts = await _contactRepository.GetContactAsync(pageSize, pageIndex);

        PaginatedItemsViewModel<Contact> contactsPaginated = new(items: contacts, count: count, pageNumber: pageIndex, pageSize: pageSize);

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(contactsPaginated.MetaData));

        return Ok(contactsPaginated);
    }

    //GET api/v1/contacts/419c092d-844e-4d9e-991a-b7ea47d04f07
    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Contact), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        Contact? contact = await _contactRepository.GetContactAsync(id);

        if (contact is null)
            return NotFound($"Contact with id: {id} is not found");

        return Ok(contact);
    }
    #endregion

    #region Post EndPoints
    //POST api/v1/contacts
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> PostContactAsync([FromBody] Contact contact)
    {
        try
        {
            _logger.LogInformation("Creating new contact with id: {id} and name: {name}",
                contact.Id, contact.Name);

            contact.CreatorUserName = User?.Claims.First().Value;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _contactRepository.AddContactAsync(contact);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Faild to create contact with id: {id} and name: {name} and exception massage is: {massage}",
                contact.Id,
                contact.Name,
                ex.Message);

            throw new ContactDomainException(ex.Message);
        }
    }
    #endregion

    #region Put EndPoints
    //PUT api/v1/contacts
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> PutContactAsync(Contact contact)
    {
        try
        {
            _logger.LogInformation("Updating contact with id: {id} and name: {name}",
                contact.Id, contact.Name);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _contactRepository.EditContactAsync(contact);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Faild to update contact with id: {id} and name: {name} and exception massage is: {massage}",
                contact.Id,
                contact.Name,
                ex.Message);

            throw new ContactDomainException(ex.Message);
        }
    }
    #endregion

    #region Delete EndPoints
    //Delete api/v1/contacts/419c092d-844e-4d9e-991a-b7ea47d04f07
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> DeleteContactAsync([FromRoute] Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting contact with id: {id}", id);

            await _contactRepository.DeleteContactAsync(id);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Faild to delete contact with id: {id} and exception massage is: {massage}",
                id,
                ex.Message);

            throw new ContactDomainException(ex.Message);
        }
    }
    #endregion
}