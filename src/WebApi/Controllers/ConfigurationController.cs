using GroupProject.Application.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

public class ConfigurationController : ApiControllerBase
{
    private readonly IConfigurationService _configuration;

    public ConfigurationController(IConfigurationService configuration) => _configuration = configuration;

    /// <summary>
    ///     Gets application configuration
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Application configuration</returns>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<ConfigurationResponse> GetConfiguration(CancellationToken cancellationToken) =>
        _configuration.Get(cancellationToken);

    /// <summary>
    ///     Updates application configuration
    /// </summary>
    /// <param name="request">Configuration to be set</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Status code</returns>
    [Authorize(Roles = "Admin")]
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> PatchConfiguration(
        PatchConfigurationRequest request,
        CancellationToken cancellationToken)
    {
        await _configuration.Patch(request, cancellationToken);
        return NoContent();
    }
}
