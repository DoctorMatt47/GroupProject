using GroupProject.Application.Common.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Requests;

public record GetTopicsParameters(
    [FromQuery] PageRequest PageRequest,
    string OrderBy,
    bool OnlyOpen,
    string? Substring,
    int? SectionId,
    Guid? UserId);
