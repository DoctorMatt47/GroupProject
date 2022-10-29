using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GroupProject.Infrastructure.Identity;

[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
public class AuthOptions
{
    private const string SecretKey = "GroupProject-SecretKey";

    public string Issuer { get; private set; } = null!;
    public string Audience { get; private set; } = null!;

    public SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.ASCII.GetBytes(SecretKey));
}
