using AutoMapper;
using GroupProject.Application.Common.Requests;
using GroupProject.Application.Common.Responses;
using GroupProject.Application.Topics;
using GroupProject.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

public class TopicsController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITopicService _topics;

    public TopicsController(ITopicService topics, IMapper mapper)
    {
        _topics = topics;
        _mapper = mapper;
    }

    /// <summary>
    ///     Gets topic by id
    /// </summary>
    /// <param name="id">Topic id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Topic with passed id</returns>
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<TopicResponse> GetTopicById(Guid id, CancellationToken cancellationToken) =>
        _topics.Get(id, cancellationToken);

    /// <summary>
    ///     Gets topics created of specific user
    /// </summary>
    /// <param name="id">User id</param>
    /// <param name="parameters">Number of elements per page and page number</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Topics created by specific user</returns>
    [AllowAnonymous]
    [HttpGet("ByUser/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<Page<TopicByUserIdResponse>> GetByUserIdOrderedByCreationTime(
        Guid id,
        [FromQuery] PageParameters parameters,
        CancellationToken cancellationToken) =>
        _topics.GetByUserIdOrderedByCreationTime(id, parameters, cancellationToken);

    /// <summary>
    ///     Gets paged information about topics ordered by creation time
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Topics page</returns>
    [AllowAnonymous]
    [HttpGet("OrderedByCreationTime")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<Page<TopicInfoResponse>> GetTopicsOrderedByCreationTime(
        [FromQuery] PageParameters parameters,
        CancellationToken cancellationToken) =>
        _topics.GetOrderedByCreationTime(parameters, cancellationToken);

    /// <summary>
    ///     Gets information about topics ordered by complaint count. Should be used in moderator menu. Is not available
    ///     for user
    /// </summary>
    /// <param name="parameters">Number of elements per page and page number</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Topics page</returns>
    [Authorize(Roles = "Moderator, Admin")]
    [HttpGet("OrderedByComplaintCount")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public Task<Page<TopicInfoResponse>> GetTopicsOrderedByComplaintCount(
        [FromQuery] PageParameters parameters,
        CancellationToken cancellationToken) =>
        _topics.GetOrderedByComplaintCount(parameters, cancellationToken);

    /// <summary>
    ///     Creates topic
    /// </summary>
    /// <param name="id">Section id</param>
    /// <param name="body">Topic parameters</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created topic id</returns>
    [Authorize(Roles = "User")]
    [HttpPost("OnSection/{id:int}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IdResponse<Guid>>> CreateTopicOnSection(
        int id,
        CreateTopicBody body,
        CancellationToken cancellationToken)
    {
        var request = _mapper.Map<CreateTopicRequest>(body) with
        {
            UserId = Guid.Parse(User.Identity?.Name!),
            SectionId = id,
        };

        var response = await _topics.Create(request, cancellationToken);
        var location = $"~api/Topics/{response.Id}";
        return Created(location, response);
    }

    [Authorize]
    [HttpPut("{id:guid}/Close")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CloseTopic(Guid id, CancellationToken cancellationToken)
    {
        await _topics.Close(id, Guid.Parse(User.Identity!.Name!), cancellationToken);
        return NoContent();
    }

    [Authorize(Roles = "Moderator, Admin")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _topics.Delete(id, cancellationToken);
        return NoContent();
    }
}
