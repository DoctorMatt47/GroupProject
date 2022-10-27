using Microsoft.IdentityModel.Tokens;

namespace GroupProject.Infrastructure.Identity;

public interface IAuthOptions
{
    string Issuer { get; }
    string Audience { get; }
    SymmetricSecurityKey GetSymmetricSecurityKey();
}
