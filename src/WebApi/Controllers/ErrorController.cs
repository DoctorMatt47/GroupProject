using GroupProject.Application.Common.Exceptions;
using GroupProject.WebApi.Responses;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ApplicationException = GroupProject.Application.Common.Exceptions.ApplicationException;

namespace GroupProject.WebApi.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    [Route("/error")]
    public ActionResult<ErrorResponse> Handle()
    {
        var error = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (error is not ApplicationException exception) return StatusCode(500);

        var responseCode = exception switch
        {
            BadRequestException => 400,
            ForbiddenException => 403,
            NotFoundException => 404,
            ConflictException => 409,
            _ => 500,
        };

        return StatusCode(
            responseCode,
            new ErrorResponse(
                exception.Message,
                exception.HowToFix,
                exception.HowToPrevent));
    }
}
