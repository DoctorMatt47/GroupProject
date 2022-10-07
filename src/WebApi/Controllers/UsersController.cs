using GroupProject.Application.Topics;
using GroupProject.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

public class UsersController : ApiControllerBase
{
    private readonly ITopicService _topics;
    private readonly IUserService _users;

    public UsersController(IUserService users, ITopicService topics)
    {
        _users = users;
        _topics = topics;
    }

    [HttpPost("Authenticate")]
    public Task<AuthenticateUserResponse> Authenticate(
        AuthenticateUserRequest request,
        CancellationToken cancellationToken) =>
        _users.Authenticate(request, cancellationToken);

    [HttpGet("{id:guid}/Topics")]
    public Task<IEnumerable<TopicByUserIdResponse>> GetByUserId(Guid id, CancellationToken cancellationToken) =>
        _topics.GetByUserId(id, cancellationToken);

    [HttpPost]
    public async Task<ActionResult<AuthenticateUserResponse>> CreateUser(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _users.CreateUser(request, cancellationToken);
        return Created(response.Id.ToString(), response);
    }

    [HttpPost("Moderator")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AuthenticateUserResponse>> CreateModerator(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _users.CreateModerator(request, cancellationToken);
        return Created(response.Id.ToString(), response);
    }
}
