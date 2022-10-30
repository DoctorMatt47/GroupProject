using AutoMapper;
using GroupProject.Application.Common.Responses;
using GroupProject.Application.Complaints;
using GroupProject.Domain.Enums;
using GroupProject.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

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
    ///     Gets complains by topic id. Should be used in moderator menu. Is not available for user
    /// </summary>
    /// <param name="id">Topic id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Complaints of topic with passed id</returns>
    [Authorize(Roles = "Moderator, Admin")]
    [HttpGet("ByTopic/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IEnumerable<ComplaintResponse>> GetComplaintsByTopic(
        Guid id,
        CancellationToken cancellationToken) =>
        _complaints.GetByTopicId(id, cancellationToken);

    /// <summary>
    ///     Gets complains by commentary id. Should be used in moderator menu. Is not available for user
    /// </summary>
    /// <param name="id">Commentary id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Complaints of commentary with passed id</returns>
    [Authorize(Roles = "User")]
    [HttpGet("ByCommentary/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IEnumerable<ComplaintResponse>> GetComplaintsByCommentary(
        Guid id,
        CancellationToken cancellationToken) =>
        _complaints.GetByCommentaryId(id, cancellationToken);

    /// <summary>
    ///     Creates complaint for specific topic
    /// </summary>
    /// <param name="id">Topic id</param>
    /// <param name="body">Complaint parameters</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created complaint id</returns>
    [Authorize(Roles = "User")]
    [HttpPost("OnTopic/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IdResponse<Guid>>> CreateComplaintOnTopic(
        Guid id,
        CreateComplaintBody body,
        CancellationToken cancellationToken)
    {
        var request = _mapper.Map<CreateComplaintRequest>(body) with {ElementId = id};
        var response = await _complaints.Create(request, cancellationToken);
        return Created(string.Empty, response);
    }

    /// <summary>
    ///     Creates complaint for specific topic
    /// </summary>
    /// <param name="id">Topic id</param>
    /// <param name="body">Complaint parameters</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created complaint id</returns>
    [Authorize(Roles = "User")]
    [HttpPost("OnCommentary/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IdResponse<Guid>>> CreateComplaintOnCommentary(
        Guid id,
        CreateComplaintBody body,
        CancellationToken cancellationToken)
    {
        var request = _mapper.Map<CreateComplaintRequest>(body) with
        {
            Target = ComplaintTarget.Commentary,
            ElementId = id,
        };

        var response = await _complaints.Create(request, cancellationToken);
        return Created(string.Empty, response);
    }
}
