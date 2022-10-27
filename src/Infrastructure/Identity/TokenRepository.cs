using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GroupProject.Application.Common.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace GroupProject.Infrastructure.Identity;

public class TokenRepository : ITokenRepository
{
    private readonly IAuthOptions _options;

    public TokenRepository(IAuthOptions options) => _options = options;

    public string Get(ClaimsIdentity identity)
    {
        var jwt = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            identity.Claims,
            DateTime.UtcNow,
            null,
            new SigningCredentials(
                _options.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
