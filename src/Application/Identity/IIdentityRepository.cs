using System.Security.Claims;
using GroupProject.Application.Common.Requests;

namespace GroupProject.Application.Identity;

public interface IIdentityRepository
{
    ClaimsIdentity Create(CreateIdentityRequest request);
}
