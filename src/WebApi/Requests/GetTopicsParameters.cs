using GroupProject.Application.Common.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Requests;

public record GetTopicsParameters(
    string? Substring,
    int? SectionId,
    Guid? UserId,
    string OrderBy,
    [FromQuery] PageRequest PageRequest,
    bool IsVerificationRequired,
    bool IsOpen);
