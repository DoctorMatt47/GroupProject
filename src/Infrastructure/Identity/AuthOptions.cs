using System.Text;
using GroupProject.Application.Identity;
using Microsoft.IdentityModel.Tokens;

namespace GroupProject.Infrastructure.Identity;

public class AuthOptions : IAuthOptions
{
    private const string Key = "GroupProject-SecretKey";

    public string Issuer => "GroupProject-SecretKey";
    public string Audience => "GroupProject-SecretKey";

    public SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.ASCII.GetBytes(Key));
}
