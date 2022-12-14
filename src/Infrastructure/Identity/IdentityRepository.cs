using System.Security.Claims;
using GroupProject.Application.Identity;

namespace GroupProject.Infrastructure.Identity;

public class IdentityRepository : IIdentityRepository
{
    public ClaimsIdentity Create(Application.Identity.Identity request)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, request.Id.ToString()),
            new(ClaimsIdentity.DefaultRoleClaimType, Enum.GetName(request.Role)!),
        };

        var identity = new ClaimsIdentity(
            claims,
            "Token",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        return identity;
    }
}
