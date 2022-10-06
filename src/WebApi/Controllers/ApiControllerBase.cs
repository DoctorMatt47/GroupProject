using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
}
