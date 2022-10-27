using System.Text;
using GroupProject.Infrastructure.Identity;
using Microsoft.IdentityModel.Tokens;

namespace GroupProject.WebApi.Options;

public class AuthOptions : IAuthOptions
{
    private const string Key = "GroupProject-SecretKey";

    public string Issuer => "GroupProject-SecretKey";
    public string Audience => "GroupProject-SecretKey";

    public SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.ASCII.GetBytes(Key));
}
