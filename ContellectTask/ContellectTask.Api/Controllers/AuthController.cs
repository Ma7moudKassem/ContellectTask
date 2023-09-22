namespace ContellectTask.Api;

[Route("api/v1/Auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _services;
    public AuthController(IAuthService services) => _services = services;

    [HttpPost("Login")]
    public async Task<IActionResult> LogIn([FromBody] LogInModel logInModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        AuthModel result = await _services.LogIn(logInModel);

        if (!result.IsAuthenticated)
            return BadRequest(result.Message);

        return Ok(result);
    }
}