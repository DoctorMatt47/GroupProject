using GroupProject.Application.Common.Responses;
using GroupProject.Application.Topics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

public class TopicsController : ApiControllerBase
{
    private readonly ITopicService _topics;

    public TopicsController(ITopicService topics) => _topics = topics;

    [HttpGet]
    public Task<IEnumerable<TopicInfoResponse>> Get(CancellationToken cancellationToken) =>
        _topics.Get(cancellationToken);

    [HttpGet("{id:guid}")]
    public Task<TopicResponse> Get(Guid id, CancellationToken cancellationToken) => _topics.Get(id, cancellationToken);

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<IdResponse<Guid>>> Create(
        CreateTopicRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _topics.Create(request, Guid.Parse(User.Identity?.Name!), cancellationToken);
        return Created(response.Id.ToString(), response);
    }
}
