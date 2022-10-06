using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GroupProject.Domain.Enums;
using Microsoft.IdentityModel.Tokens;

namespace GroupProject.Application.Identity;

public class JwtTokenService
{
    private readonly IAuthOptions _options;

    public JwtTokenService(IAuthOptions options) => _options = options;

    public string Get(Guid userId, UserRole role)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, userId.ToString()),
            new(ClaimsIdentity.DefaultRoleClaimType, Enum.GetName(role)!),
        };

        var identity = new ClaimsIdentity(
            claims,
            "Token",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

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
