using Microsoft.IdentityModel.Tokens;

namespace GroupProject.Application.Identity;

public interface IAuthOptions
{
    string Issuer { get; }
    string Audience { get; }
    SymmetricSecurityKey GetSymmetricSecurityKey();
}
