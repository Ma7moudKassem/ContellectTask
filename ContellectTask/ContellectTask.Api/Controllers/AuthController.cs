namespace ContellectTask.Api;

[Route("api/v1/Auth")]
[ApiController]
public class AuthController : ControllerBase
{
    readonly IAuthService _services;
    readonly ILogger<AuthController> _logger;
    public AuthController(IAuthService services, ILogger<AuthController> logger)
    {
        _services = services;
        _logger = logger;
    }

    //POST api/auth/v1/login
    [HttpPost("Login")]
    [ProducesResponseType(typeof(AuthModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> LogIn([FromBody] LogInModel logInModel)
    {
        _logger.LogInformation("Login by {userName}", logInModel.UserName);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        AuthModel result = await _services.LogIn(logInModel);

        if (!result.IsAuthenticated)
            return BadRequest(result.Message);

        return Ok(result);
    }
}