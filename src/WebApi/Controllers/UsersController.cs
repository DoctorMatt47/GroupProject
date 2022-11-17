using GroupProject.Application.Common.Requests;
using GroupProject.Application.Common.Responses;
using GroupProject.Application.Identity;
using GroupProject.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

public class UsersController : ApiControllerBase
{
    private readonly IUserService _users;

    public UsersController(IUserService users) => _users = users;

    [Authorize("Moderator, Admin")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<Page<UserResponse>> GetUsers(
        [FromQuery] PageRequest request,
        CancellationToken cancellationToken) =>
        await _users.GetUsers(request, cancellationToken);

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

    [Authorize(Roles = "Admin")]
    [HttpGet("Moderators")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<Page<UserResponse>> GetModerators(
        [FromQuery] PageRequest request,
        CancellationToken cancellationToken) =>
        await _users.GetModerators(request, cancellationToken);

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
    [HttpPost("Moderators")]
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

    [Authorize(Roles = "Moderator, Admin")]
    [HttpPost("Ban/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> BanUser(Guid id, CancellationToken cancellationToken)
    {
        await _users.BanUser(id, cancellationToken);
        return NoContent();
    }
}
