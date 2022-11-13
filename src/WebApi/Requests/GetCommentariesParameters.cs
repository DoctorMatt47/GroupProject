using GroupProject.Application.Common.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Requests;

public record GetCommentariesParameters(
    [FromQuery] PageRequest PageRequest,
    string OrderBy,
    Guid? UserId);
