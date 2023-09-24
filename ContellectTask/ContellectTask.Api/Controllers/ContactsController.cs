namespace ContellectTask.Api;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class ContactsController : ControllerBase
{
    readonly IContactRepository _repository;
    readonly ILogger<ContactsController> _logger;
    readonly IValidator<Contact> _validator;
    public ContactsController(IContactRepository repository, ILogger<ContactsController> logger, IValidator<Contact> validator)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
    }

    #region Get EndPoints
    //GET api/v1/contacts?pageSize=5&pageIndex=1
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedItemsViewModel<Contact>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllAsync([FromQuery] int pageSize = 5, [FromQuery] int pageIndex = 1)
    {
        _logger.LogInformation("Getting all contacts...");

        long count = await _repository.CountAsync();

        IEnumerable<Contact> contacts = await _repository.GetContactAsync(pageSize, pageIndex);

        PaginatedItemsViewModel<Contact> contactsPaginated = new(items: contacts, count: count, pageNumber: pageIndex, pageSize: pageSize);

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(contactsPaginated.MetaData));

        return Ok(contactsPaginated);
    }

    //GET api/v1/contacts/419c092d-844e-4d9e-991a-b7ea47d04f07
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Contact), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        _logger.LogInformation("Getting contact with id: {id} ...", id);

        Contact? contact = await _repository.GetContactAsync(id);

        if (contact is null)
            return NotFound($"Contact with id: {id} is not found");

        return Ok(contact);
    }
    #endregion

    #region Post EndPoints
    //POST api/v1/contacts
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(List<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> PostContactAsync([FromBody] Contact contact)
    {
        try
        {
            _logger.LogInformation("Creating new contact with id: {id} and name: {name}",
                contact.Id, contact.Name);

            contact.CreatorUserName = User?.Claims.First().Value;

            ValidationResult result = await _validator.ValidateAsync(contact);

            if (!result.IsValid)
                return BadRequest(result.Errors);

            await _repository.AddContactAsync(contact);

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
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(List<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> PutContactAsync(Contact contact)
    {
        try
        {
            _logger.LogInformation("Updating contact with id: {id} and name: {name}",
                contact.Id, contact.Name);

            ValidationResult result = await _validator.ValidateAsync(contact);

            if (!result.IsValid)
                return BadRequest(result.Errors);

            Contact? contactFromDb = await _repository.GetContactAsync(contact.Id);

            if (contactFromDb?.CreatorUserName != User.Claims.First().Value)
                return BadRequest("You can't edit this contact because you aren't creator");

            await _repository.EditContactAsync(contact);

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
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteContactAsync([FromRoute] Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting contact with id: {id}", id);

            Contact? contact = await _repository.GetContactAsync(id);

            if (contact is null)
                return NotFound($"Contact with id: {id} is not found");

            if (contact?.CreatorUserName != User.Claims.First().Value)
                return BadRequest("You can't delete this contact because you aren't creator");

            await _repository.DeleteContactAsync(id);

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