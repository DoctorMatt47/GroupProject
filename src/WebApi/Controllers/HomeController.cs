using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

[Route("/")]
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : ControllerBase
{
    //public ActionResult RedirectToSwagger() => Redirect("/swagger/index.html");
}
