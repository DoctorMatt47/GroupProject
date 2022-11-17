using AutoMapper;
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

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<Page<TopicHeaderResponse>>> GetTopics(
        [FromQuery] GetTopicsParameters parameters,
        CancellationToken cancellationToken)
    {
        var request = _mapper.Map<GetTopicsRequest>(parameters);
        if (request.OrderBy is not (TopicsOrderedBy.VerifyBefore or TopicsOrderedBy.ComplaintCount))
            return await _topics.Get(request, cancellationToken);

        if (!User.IsInRole("Moderator") && !User.IsInRole("Admin")) return Forbid();
        return await _topics.Get(request, cancellationToken);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<TopicResponse>> GetTopic(
        Guid id,
        CancellationToken cancellationToken) =>
        await _topics.Get(id, cancellationToken);

    /// <summary>
    ///     Creates topic
    /// </summary>
    /// <param name="id">Section id</param>
    /// <param name="body">Topic parameters</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created topic id</returns>
    [Authorize(Roles = "User", Policy = "NotBanned")]
    [HttpPost("InSection/{id:int}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IdResponse<Guid>>> CreateTopicInSection(
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

    [Authorize(Roles = "Moderator, Admin")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteTopic(Guid id, CancellationToken cancellationToken)
    {
        await _topics.Delete(id, cancellationToken);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost("{id:guid}/View")]
    public async Task<ActionResult> ViewTopic(Guid id, CancellationToken cancellationToken)
    {
        await _topics.View(id, cancellationToken);
        return NoContent();
    }

    [Authorize(Roles = "Moderator, Admin")]
    [HttpPut("{id:guid}/Verify")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> VerifyTopic(Guid id, CancellationToken cancellationToken)
    {
        await _topics.Verify(id, cancellationToken);
        return NoContent();
    }

    [Authorize(Policy = "NotBanned")]
    [HttpPut("{id:guid}/Close")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CloseTopic(Guid id, CancellationToken cancellationToken)
    {
        await _topics.Close(id, Guid.Parse(User.Identity!.Name!), cancellationToken);
        return NoContent();
    }
}
