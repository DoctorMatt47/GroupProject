using GroupProject.Application.Users;

namespace GroupProject.Application.Identity;

public interface IIdentityService
{
    Task<IdentityResponse> Create(AuthenticateUserRequest request, CancellationToken cancellationToken);
}
