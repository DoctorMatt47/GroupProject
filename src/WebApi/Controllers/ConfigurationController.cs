using GroupProject.Application.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace GroupProject.WebApi.Controllers;

public class ConfigurationController : ApiControllerBase
{
    private readonly IConfigurationService _configuration;

    public ConfigurationController(IConfigurationService configuration) => _configuration = configuration;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<ConfigurationResponse> GetConfiguration(CancellationToken cancellationToken) =>
        _configuration.Get(cancellationToken);

    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> PatchConfiguration(
        PatchConfigurationRequest request,
        CancellationToken cancellationToken)
    {
        await _configuration.Patch(request, cancellationToken);
        return NoContent();
    }
}
