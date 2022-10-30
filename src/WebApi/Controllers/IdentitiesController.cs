using GroupProject.Application.Identity;
using GroupProject.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

public class IdentitiesController : ApiControllerBase
{
    private readonly IIdentityService _identities;

    public IdentitiesController(IIdentityService identities) => _identities = identities;

    /// <summary>
    ///     Authenticates user by returning JWT authorization token
    /// </summary>
    /// <param name="body">User login and password</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     User information and JWT token which should be passed in the 'Authentication' header in API requests that
    ///     require authentication
    /// </returns>
    [AllowAnonymous]
    [HttpPost("Authenticate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<IdentityResponse> Authenticate(
        AuthenticateUserRequest body,
        CancellationToken cancellationToken) =>
        _identities.Create(body, cancellationToken);
}
