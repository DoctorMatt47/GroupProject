using AutoMapper;
using GroupProject.Application.Commentaries;
using GroupProject.Application.Common.Responses;
using GroupProject.Application.Complaints;
using GroupProject.Application.Topics;
using GroupProject.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

[Authorize]
public class TopicsController : ApiControllerBase
{
    private readonly ICommentaryService _commentaries;
    private readonly IComplaintService _complaints;
    private readonly IMapper _mapper;
    private readonly ITopicService _topics;

    public TopicsController(
        ITopicService topics,
        IComplaintService complaints,
        ICommentaryService commentaries,
        IMapper mapper)
    {
        _topics = topics;
        _complaints = complaints;
        _commentaries = commentaries;
        _mapper = mapper;
    }

    /// <summary>
    ///     Gets paged information about topics ordered by creation time
    /// </summary>
    /// <param name="perPage">Number of topics per page</param>
    /// <param name="page">Page number</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Topics page</returns>
    [AllowAnonymous]
    [HttpGet("OrderedByCreationTime")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<Page<TopicInfoForUserResponse>> GetTopicsOrderedByCreationTime(
        int perPage,
        int page,
        CancellationToken cancellationToken) =>
        _topics.GetOrderedByCreationTime(perPage, page, cancellationToken);

    /// <summary>
    ///     Gets information about topics ordered by complaint count. Should be used in moderator menu. Is not available
    ///     for user
    /// </summary>
    /// <param name="perPage">Number of topics per page</param>
    /// <param name="page">Page number</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Topics page</returns>
    [Authorize(Roles = "Moderator, Admin")]
    [HttpGet("OrderedByComplaintCount")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public Task<Page<TopicInfoForModeratorResponse>> GetTopicsOrderedByComplaintCount(
        int perPage,
        int page,
        CancellationToken cancellationToken) =>
        _topics.GetOrderedByComplaintCount(perPage, page, cancellationToken);

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
    ///     Gets complains by topic id. Should be used in moderator menu. Is not available for user.
    /// </summary>
    /// <param name="id">Topic id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Complaints of topic with passed id</returns>
    [Authorize(Roles = "Moderator, Admin")]
    [HttpGet("{id:guid}/Complaints")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IEnumerable<ComplaintResponse>> GetComplaintsByTopicId(
        Guid id,
        CancellationToken cancellationToken) =>
        _complaints.GetByTopicId(id, cancellationToken);

    /// <summary>
    ///     Gets paged commentaries by topic id
    /// </summary>
    /// <param name="id">Topic id</param>
    /// <param name="perPage">Number of commentary per page</param>
    /// <param name="page">Number of pages</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Commentary page</returns>
    [AllowAnonymous]
    [HttpGet("{id:guid}/Commentaries")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<Page<CommentaryResponse>> GetCommentariesByTopicId(
        Guid id,
        int perPage,
        int page,
        CancellationToken cancellationToken) =>
        _commentaries.GetByTopicId(id, perPage, page, cancellationToken);

    /// <summary>
    ///     Creates complaint for specific topic
    /// </summary>
    /// <param name="id">Topic id</param>
    /// <param name="body">Complaint parameters</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created complaint id</returns>
    [Authorize(Roles = "User")]
    [HttpPost("{id:guid}/Complaints")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IdResponse<Guid>>> CreateComplaintOnTopic(
        Guid id,
        CreateComplaintBody body,
        CancellationToken cancellationToken)
    {
        var request = _mapper.Map<CreateComplaintRequest>(body) with {TopicId = id};
        var response = await _complaints.CreateComplaint(request, cancellationToken);
        return Created(string.Empty, response);
    }

    /// <summary>
    ///     Creates commentary for specific topic
    /// </summary>
    /// <param name="id">Topic id</param>
    /// <param name="body">Commentary parameters</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created commentary id</returns>
    [Authorize(Roles = "User")]
    [HttpPost("{id:guid}/Commentaries")]
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
            UserId = Guid.Parse(User.Identity?.Name!),
        };

        var response = await _commentaries.Create(request, cancellationToken);
        return Created(string.Empty, response);
    }
}
