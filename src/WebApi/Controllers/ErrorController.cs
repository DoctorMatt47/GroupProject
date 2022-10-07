using GroupProject.Application.Common.Exceptions;
using GroupProject.WebApi.Responses;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    [Route("/error")]
    public ActionResult<ErrorResponse> Handle()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        var responseCode = exception switch
        {
            BadRequestException => 400,
            UnauthorizedException => 401,
            ForbiddenException => 403,
            NotFoundException => 404,
            ConflictException => 409,
            _ => 500,
        };

        return StatusCode(responseCode, new ErrorResponse(exception?.Message, exception?.StackTrace));
    }
}