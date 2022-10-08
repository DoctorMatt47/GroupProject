using GroupProject.Application.Topics;
using GroupProject.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

[Authorize]
public class UsersController : ApiControllerBase
{
    private readonly ITopicService _topics;
    private readonly IUserService _users;

    public UsersController(IUserService users, ITopicService topics)
    {
        _users = users;
        _topics = topics;
    }

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
    public Task<AuthenticateUserResponse> Authenticate(
        AuthenticateUserRequest body,
        CancellationToken cancellationToken) =>
        _users.Authenticate(body, cancellationToken);

    /// <summary>
    ///     Gets topics created of specific user
    /// </summary>
    /// <param name="id">User id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Topics created by specific user</returns>
    [AllowAnonymous]
    [HttpGet("{id:guid}/Topics")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IEnumerable<TopicByUserIdResponse>> GetByUserId(Guid id, CancellationToken cancellationToken) =>
        _topics.GetByUserId(id, cancellationToken);

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
    public async Task<ActionResult<AuthenticateUserResponse>> CreateUser(
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
    public async Task<ActionResult<AuthenticateUserResponse>> CreateModerator(
        CreateUserRequest body,
        CancellationToken cancellationToken)
    {
        var response = await _users.CreateModerator(body, cancellationToken);
        return Created("", response);
    }
}
