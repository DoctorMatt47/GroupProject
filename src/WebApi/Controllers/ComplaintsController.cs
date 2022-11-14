using AutoMapper;
using GroupProject.Application.Common.Requests;
using GroupProject.Application.Common.Responses;
using GroupProject.Application.Complaints;
using GroupProject.Domain.Enums;
using GroupProject.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
public class ComplaintsController : ApiControllerBase
{
    private readonly IComplaintService _complaints;
    private readonly IMapper _mapper;

    public ComplaintsController(
        IComplaintService complaints,
        IMapper mapper)
    {
        _complaints = complaints;
        _mapper = mapper;
    }

    /// <summary>
    ///     Gets paged complaints
    /// </summary>
    /// <param name="request">Number of elements per page and page number</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Paged complaints</returns>
    [Authorize(Roles = "Moderator, Admin")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<Page<ComplaintResponse>> GetComplaints(
        [FromQuery] PageRequest request,
        CancellationToken cancellationToken) =>
        _complaints.Get(request, cancellationToken);

    [Authorize(Roles = "Moderator, Admin")]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ComplaintResponse> Get(Guid id, CancellationToken cancellationToken) =>
        _complaints.Get(id, cancellationToken);


    /// <summary>
    ///     Gets complaints by topic id. Should be used in moderator menu. Is not available for user
    /// </summary>
    /// <param name="id">Topic id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Complaints of topic with passed id</returns>
    [Authorize(Roles = "Moderator, Admin")]
    [HttpGet("AboutTopic/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IEnumerable<ComplaintByTargetResponse>> GetComplaintsAboutTopic(
        Guid id,
        CancellationToken cancellationToken) =>
        _complaints.GetByTopicId(id, cancellationToken);

    /// <summary>
    ///     Gets complaints by commentary id. Should be used in moderator menu. Is not available for user
    /// </summary>
    /// <param name="id">Commentary id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Complaints of commentary with passed id</returns>
    [Authorize(Roles = "Moderator, Admin")]
    [HttpGet("AboutCommentary/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IEnumerable<ComplaintByTargetResponse>> GetComplaintsAboutCommentary(
        Guid id,
        CancellationToken cancellationToken) =>
        _complaints.GetByCommentaryId(id, cancellationToken);

    /// <summary>
    ///     Creates complaint about specific topic
    /// </summary>
    /// <param name="id">Topic id</param>
    /// <param name="body">Complaint parameters</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created complaint id</returns>
    [Authorize]
    [HttpPost("AboutTopic/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IdResponse<Guid>>> CreateComplaintAboutTopic(
        Guid id,
        CreateComplaintBody body,
        CancellationToken cancellationToken)
    {
        var request = _mapper.Map<CreateComplaintRequest>(body) with
        {
            Target = ComplaintTarget.Topic,
            TargetId = id,
            UserId = Guid.Parse(User.Identity?.Name!),
        };

        var response = await _complaints.Create(request, cancellationToken);
        return Created(string.Empty, response);
    }

    /// <summary>
    ///     Creates complaint about specific commentary
    /// </summary>
    /// <param name="id">Topic id</param>
    /// <param name="body">Complaint parameters</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created complaint id</returns>
    [Authorize]
    [HttpPost("AboutCommentary/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IdResponse<Guid>>> CreateComplaintAboutCommentary(
        Guid id,
        CreateComplaintBody body,
        CancellationToken cancellationToken)
    {
        var request = _mapper.Map<CreateComplaintRequest>(body) with
        {
            Target = ComplaintTarget.Commentary,
            TargetId = id,
            UserId = Guid.Parse(User.Identity?.Name!),
        };

        var response = await _complaints.Create(request, cancellationToken);
        return Created(string.Empty, response);
    }

    [Authorize(Roles = "Moderator, Admin")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _complaints.Delete(id, cancellationToken);
        return NoContent();
    }
}
