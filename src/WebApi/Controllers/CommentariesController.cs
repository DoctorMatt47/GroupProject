using GroupProject.Application.Commentaries;
using GroupProject.Application.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

public class CommentariesController : ApiControllerBase
{
    private readonly ICommentaryService _commentaries;

    public CommentariesController(ICommentaryService commentaries) => _commentaries = commentaries;

    [HttpGet("{id:guid}")]
    public Task<IEnumerable<CommentaryResponse>> GetByTopicId(Guid id, CancellationToken cancellationToken) =>
        _commentaries.GetByTopicId(id, cancellationToken);

    [HttpPost]
    public async Task<ActionResult<IdResponse<Guid>>> Create(
        CreateCommentaryRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _commentaries.Create(request, Guid.Parse(User.Identity?.Name!), cancellationToken);
        return Created(response.Id.ToString(), response);
    }
}
