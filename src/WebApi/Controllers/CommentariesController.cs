using AutoMapper;
using GroupProject.Application.Commentaries;
using GroupProject.Application.Common.Requests;
using GroupProject.Application.Common.Responses;
using GroupProject.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

public class CommentariesController : ApiControllerBase
{
    private readonly ICommentaryService _commentaries;
    private readonly IMapper _mapper;

    public CommentariesController(
        ICommentaryService commentaries,
        IMapper mapper)
    {
        _commentaries = commentaries;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<CommentaryResponse> Get(Guid id, CancellationToken cancellationToken) =>
        await _commentaries.Get(id, cancellationToken);

    [AllowAnonymous]
    [HttpGet("ByUser/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<Page<CommentaryByUserResponse>> GetCommentariesByUserIdOrderedByCreationTime(
        Guid id,
        [FromQuery] PageParameters parameters,
        CancellationToken cancellationToken) =>
        _commentaries.GetByUserIdOrderedByCreationTime(id, parameters, cancellationToken);

    /// <summary>
    ///     Gets paged commentaries by topic id
    /// </summary>
    /// <param name="id">Topic id</param>
    /// <param name="parameters">Number of elements per page and page number</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Commentary page</returns>
    [AllowAnonymous]
    [HttpGet("ByTopic/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<Page<CommentaryResponse>> GetCommentariesByTopicIdOrderedByCreationTime(
        Guid id,
        [FromQuery] PageParameters parameters,
        CancellationToken cancellationToken) =>
        _commentaries.GetByTopicIdOrderedByCreationTime(id, parameters, cancellationToken);

    /// <summary>
    ///     Gets paged commentaries ordered by complaint count
    /// </summary>
    /// <param name="parameters">Number of elements per page and page number</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Commentary page</returns>
    [AllowAnonymous]
    [HttpGet("OrderedByComplaintCount")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<Page<CommentaryResponse>> GetCommentariesOrderedByComplaintCount(
        [FromQuery] PageParameters parameters,
        CancellationToken cancellationToken) =>
        _commentaries.GetOrderedByComplaintCount(parameters, cancellationToken);

    /// <summary>
    ///     Creates commentary for specific topic
    /// </summary>
    /// <param name="id">Topic id</param>
    /// <param name="body">Commentary parameters</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created commentary id</returns>
    [Authorize(Roles = "User")]
    [HttpPost("OnTopic/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IdResponse<Guid>>> CreateCommentaryOnTopic(
        Guid id,
        CreateCommentaryBody body,
        CancellationToken cancellationToken)
    {
        var request = _mapper.Map<CreateCommentaryRequest>(body) with
        {
            TopicId = id,
            UserId = Guid.Parse(User.Identity?.Name!)
        };

        var response = await _commentaries.Create(request, cancellationToken);
        return Created(string.Empty, response);
    }

    [Authorize(Roles = "Moderator, Admin")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _commentaries.Delete(id, cancellationToken);
        return NoContent();
    }
}
