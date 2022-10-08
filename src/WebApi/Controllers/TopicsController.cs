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

    [HttpGet("OrderedByCreationTime")]
    public Task<Page<TopicInfoForUserResponse>> GetOrderedByCreationTime(
        int perPage,
        int page,
        CancellationToken cancellationToken) =>
        _topics.GetOrderedByCreationTime(perPage, page, cancellationToken);

    [HttpGet("OrderedByComplaintCount")]
    public Task<Page<TopicInfoForUserResponse>> GetOrderedByComplaintCount(
        int perPage,
        int page,
        CancellationToken cancellationToken) =>
        _topics.GetOrderedByCreationTime(perPage, page, cancellationToken);

    [HttpGet("{id:guid}")]
    public Task<TopicResponse> Get(Guid id, CancellationToken cancellationToken) => _topics.Get(id, cancellationToken);

    [Authorize(Roles = "Moderator, Admin")]
    [HttpGet("{id:guid}/complaints")]
    public Task<IEnumerable<ComplaintResponse>> GetComplaints(Guid id, CancellationToken cancellationToken) =>
        _complaints.GetByTopicId(id, cancellationToken);

    [AllowAnonymous]
    [HttpGet("{id:guid}/Commentaries")]
    public Task<Page<CommentaryResponse>> GetCommentariesByTopicId(
        Guid id,
        int perPage,
        int page,
        CancellationToken cancellationToken) =>
        _commentaries.GetByTopicId(id, perPage, page, cancellationToken);

    [Authorize(Roles = "User")]
    [HttpPost("{id:guid}/Complaints")]
    public async Task<ActionResult<IdResponse<Guid>>> CreateComplaint(
        Guid id,
        CreateComplaintBody body,
        CancellationToken cancellationToken)
    {
        var request = _mapper.Map<CreateComplaintRequest>(body) with {TopicId = id};
        var response = await _complaints.CreateComplaint(request, cancellationToken);
        return Created("", response);
    }

    [Authorize(Roles = "User")]
    [HttpPost("{id:guid}/Commentaries")]
    public async Task<ActionResult<IdResponse<Guid>>> CreateCommentary(
        Guid id,
        CreateCommentaryBody body,
        CancellationToken cancellationToken)
    {
        var request = _mapper.Map<CreateCommentaryRequest>(body) with {TopicId = id};
        var response = await _commentaries.Create(request, cancellationToken);
        var location = new Uri($"~api/Topics/{response.Id}");
        return Created(location, response);
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<ActionResult<IdResponse<Guid>>> Create(
        CreateTopicRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _topics.Create(request, cancellationToken);
        var location = new Uri($"~api/Topics/{response.Id}");
        return Created(location, response);
    }
}
