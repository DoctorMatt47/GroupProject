using GroupProject.Application.Common.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Requests;

public record GetCommentariesParameters(
    Guid? UserId,
    string OrderBy,
    [FromQuery] PageRequest PageRequest);
