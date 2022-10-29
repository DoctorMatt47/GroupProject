using AutoMapper;
using GroupProject.Application.Common.Responses;
using GroupProject.Application.Sections;
using GroupProject.Application.Topics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

public class SectionController : ApiControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISectionService _sections;
    private readonly ITopicService _topics;

    public SectionController(ISectionService sections, IMapper mapper, ITopicService topics)
    {
        _sections = sections;
        _mapper = mapper;
        _topics = topics;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IEnumerable<SectionResponse>> Get(CancellationToken cancellationToken) =>
        _sections.Get(cancellationToken);


    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IdResponse<Guid>>> Create(
        CreateSectionRequest body,
        CancellationToken cancellationToken)
    {
        var response = await _sections.Create(body, cancellationToken);
        return Created(string.Empty, response);
    }
}
