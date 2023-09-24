namespace ContellectTask.Api;

[Route("api/v1/Auth")]
[ApiController]
public class AuthController : ControllerBase
{
    readonly IAuthService _services;
    readonly ILogger<AuthController> _logger;
    readonly IValidator<LogInModel> _validator;
    public AuthController(IAuthService services, ILogger<AuthController> logger, IValidator<LogInModel> validator)
    {
        _services = services;
        _logger = logger;
        _validator = validator;
    }

    //POST api/auth/v1/login
    [HttpPost("Login")]
    [ProducesResponseType(typeof(AuthModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(List<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> LogIn([FromBody] LogInModel logInModel)
    {
        _logger.LogInformation("Login by {userName}", logInModel.UserName);

        ValidationResult result = await _validator.ValidateAsync(logInModel);

        if (!result.IsValid)
            return BadRequest(result.Errors);

        AuthModel auth = await _services.LogIn(logInModel);

        if (!auth.IsAuthenticated)
            return BadRequest(auth.Message);

        return Ok(auth);
    }
}