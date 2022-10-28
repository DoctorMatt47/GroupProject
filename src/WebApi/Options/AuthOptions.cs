using System.Text;
using GroupProject.Infrastructure.Identity;
using Microsoft.IdentityModel.Tokens;

namespace GroupProject.WebApi.Options;

public class AuthOptions : IAuthOptions
{
    private const string Key = "GroupProject-SecretKey";

    public string Issuer => "GroupProject-Issuer";
    public string Audience => "GroupProject-Audience";

    public SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.ASCII.GetBytes(Key));
}
