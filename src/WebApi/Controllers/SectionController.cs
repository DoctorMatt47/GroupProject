﻿using AutoMapper;
using GroupProject.Application.Common.Responses;
using GroupProject.Application.Sections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

public class SectionController : ApiControllerBase
{
    private readonly ISectionService _sections;

    public SectionController(ISectionService sections, IMapper mapper) => _sections = sections;

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
}
