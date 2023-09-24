namespace ContellectTask.Infrastructure;

public class AuthService : IAuthService
{
    readonly JWT _jwt;
    readonly UserManager<IdentityUser> _userManager;
    public AuthService(IOptions<JWT> jwt, UserManager<IdentityUser> userManager)
    {
        _jwt = jwt.Value;
        _userManager = userManager;
    }

    #region LogIn
    public async Task<AuthModel> LogIn(LogInModel logInModel)
    {
        AuthModel authModel = new();

        IdentityUser? user = await _userManager.FindByNameAsync(logInModel.UserName);

        if (user is null || !await _userManager.CheckPasswordAsync(user, logInModel.Password))
        {
            authModel.Message = "User Name or Password is InCorrect!";

            return authModel;
        }

        JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);

        authModel.IsAuthenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        authModel.UserName = user.UserName;
        authModel.ExpiresOn = jwtSecurityToken.ValidTo;

        return authModel;
    }
    #endregion

    #region Generate JWT
    private async Task<JwtSecurityToken> CreateJwtToken(IdentityUser user)
    {
        IList<Claim> userClaims = await _userManager.GetClaimsAsync(user);

        List<Claim> roleClaims = new();

        IEnumerable<Claim> claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", user.Id)
        }
        .Union(userClaims)
        .Union(roleClaims);

        SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(_jwt.Key));
        SigningCredentials signingCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtSecurityToken = new(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_jwt.DurationInDays),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }
    #endregion
}