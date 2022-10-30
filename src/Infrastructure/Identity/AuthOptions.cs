using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GroupProject.Infrastructure.Identity;

public class AuthOptions
{
    private const string SecretKey = "GroupProject-SecretKey";

    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;

    public SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.ASCII.GetBytes(SecretKey));
}
