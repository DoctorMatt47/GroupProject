using AutoMapper;
using GroupProject.Application.Commentaries;
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
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<Page<CommentaryResponse>> Get(
        [FromQuery] GetCommentariesParameters parameters,
        CancellationToken cancellationToken)
    {
        var request = _mapper.Map<GetCommentariesRequest>(parameters);
        return await _commentaries.Get(request, cancellationToken);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<CommentaryResponse> Get(Guid id, CancellationToken cancellationToken) =>
        await _commentaries.Get(id, cancellationToken);

    /// <summary>
    ///     Creates commentary for specific topic
    /// </summary>
    /// <param name="id">Topic id</param>
    /// <param name="body">Commentary parameters</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created commentary id</returns>
    [Authorize(Policy = "NotBanned")]
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
            UserId = Guid.Parse(User.Identity?.Name!),
        };

        var response = await _commentaries.Create(request, cancellationToken);
        return Created(string.Empty, response);
    }

    [Authorize(Roles = "Moderator, Admin")]
    [HttpPost("{id:guid}/Verify")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> VerifyCommentary(Guid id, CancellationToken cancellationToken)
    {
        await _commentaries.Verify(id, cancellationToken);
        return NoContent();
    }

    [Authorize(Roles = "Moderator, Admin")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteCommentary(Guid id, CancellationToken cancellationToken)
    {
        await _commentaries.Delete(id, cancellationToken);
        return NoContent();
    }
}
