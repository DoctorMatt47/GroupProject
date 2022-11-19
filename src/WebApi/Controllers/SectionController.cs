using AutoMapper;
using GroupProject.Application.Common.Responses;
using GroupProject.Application.Sections;
using GroupProject.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

public class SectionController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISectionService _sections;

    public SectionController(ISectionService sections, IMapper mapper)
    {
        _sections = sections;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IEnumerable<SectionResponse>> Get(CancellationToken cancellationToken) =>
        _sections.Get(cancellationToken);

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IdResponse<Guid>>> Create(
        CreateSectionRequest body,
        CancellationToken cancellationToken)
    {
        var response = await _sections.Create(body, cancellationToken);
        return Created(string.Empty, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _sections.Delete(id, cancellationToken);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Update(int id, PutSectionBody body, CancellationToken cancellationToken)
    {
        var request = _mapper.Map<PutSectionRequest>(body) with {Id = id};
        await _sections.Update(request, cancellationToken);
        return NoContent();
    }
}
