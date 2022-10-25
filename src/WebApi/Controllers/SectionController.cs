using AutoMapper;
using GroupProject.Application.Common.Responses;
using GroupProject.Application.Sections;
using GroupProject.Application.Topics;
using GroupProject.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

[Authorize]
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

    /// <summary>
    ///     Creates topic
    /// </summary>
    /// <param name="id">Section id</param>
    /// <param name="body">Topic parameters</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created topic id</returns>
    [Authorize(Roles = "User")]
    [HttpPost("{id:int}/Topics")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IdResponse<Guid>>> CreateTopic(
        int id,
        CreateTopicBody body,
        CancellationToken cancellationToken)
    {
        var request = _mapper.Map<CreateTopicRequest>(body) with
        {
            UserId = Guid.Parse(User.Identity?.Name!),
            SectionId = id,
        };

        var response = await _topics.Create(request, cancellationToken);
        var location = $"~api/Topics/{response.Id}";
        return Created(location, response);
    }
}
