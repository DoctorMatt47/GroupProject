using System.Security.Claims;
using GroupProject.Application.Common.Requests;
using GroupProject.Application.Identity;

namespace GroupProject.Infrastructure.Identity;

public class IdentityRepository : IIdentityRepository
{
    public ClaimsIdentity Create(CreateIdentityRequest request)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, request.UserId.ToString()),
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
