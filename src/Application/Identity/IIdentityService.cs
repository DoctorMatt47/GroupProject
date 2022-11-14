namespace GroupProject.Application.Identity;

public interface IIdentityService
{
    Task<IdentityResponse> Create(CreateIdentityRequest request, CancellationToken cancellationToken);
}
