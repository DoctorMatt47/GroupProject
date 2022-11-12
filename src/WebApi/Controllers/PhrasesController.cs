using GroupProject.Application.Phrases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

[Authorize(Roles = "Admin")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
public class PhrasesController : ApiControllerBase
{
    private readonly IPhraseService _phrases;

    public PhrasesController(IPhraseService phrases) => _phrases = phrases;

    [HttpGet("Forbidden")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IEnumerable<PhraseResponse>> GetForbiddenPhrases(CancellationToken cancellationToken) =>
        _phrases.GetForbidden(cancellationToken);

    [HttpGet("VerificationRequired")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IEnumerable<PhraseResponse>> GetVerificationRequired(CancellationToken cancellationToken) =>
        _phrases.GetVerificationRequired(cancellationToken);

    [HttpGet("Forbidden/Bulk")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> PutForbiddenPhrases(
        IEnumerable<PutPhraseRequest> request,
        CancellationToken cancellationToken)
    {
        await _phrases.UpdateForbidden(request, cancellationToken);
        return NoContent();
    }

    [HttpGet("VerificationRequired/Bulk")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> PutVerificationRequired(
        IEnumerable<PutPhraseRequest> request,
        CancellationToken cancellationToken)
    {
        await _phrases.UpdateVerificationRequired(request, cancellationToken);
        return NoContent();
    }
}
