using System.Security.Claims;

namespace GroupProject.Application.Identity;

public interface IIdentityRepository
{
    ClaimsIdentity Create(Identity request);
}
