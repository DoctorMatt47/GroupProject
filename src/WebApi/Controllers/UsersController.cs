using GroupProject.Application.Identity;
using GroupProject.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

public class UsersController : ApiControllerBase
{
    private readonly IUserService _users;

    public UsersController(IUserService users) => _users = users;

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<UserResponse> GetUser(Guid id, CancellationToken cancellationToken) =>
        await _users.Get(id, cancellationToken);

    /// <summary>
    ///     Creates new user with passed parameters
    /// </summary>
    /// <param name="body">Users parameters</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     Created user information and JWT token which should be passed in the 'Authentication' header in API requests
    ///     that require authentication
    /// </returns>
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IdentityResponse>> CreateUser(
        CreateUserRequest body,
        CancellationToken cancellationToken)
    {
        var response = await _users.CreateUser(body, cancellationToken);
        return Created("", response);
    }

    /// <summary>
    ///     Creates moderator with passed parameters. Available only for admin
    /// </summary>
    /// <param name="body">Parameters of moderator to be created</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     Created moderator information and JWT token which should be passed in the 'Authentication' header in API
    ///     requests that require authentication
    /// </returns>
    [Authorize(Roles = "Admin")]
    [HttpPost("Moderator")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IdentityResponse>> CreateModerator(
        CreateUserRequest body,
        CancellationToken cancellationToken)
    {
        var response = await _users.CreateModerator(body, cancellationToken);
        return Created("", response);
    }

    [Authorize(Roles = "Moderator, Admin")]
    [HttpPost("{id:guid}/Warning")]
    public async Task<ActionResult> AddWarningToUser(Guid id, CancellationToken cancellationToken)
    {
        await _users.AddWarningToUser(id, cancellationToken);
        return NoContent();
    }
}
