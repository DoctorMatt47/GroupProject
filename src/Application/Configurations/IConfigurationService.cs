namespace GroupProject.Application.Configurations;

public interface IConfigurationService
{
    Task<ConfigurationResponse> Get(CancellationToken cancellationToken);
    Task Patch(PatchConfigurationRequest request, CancellationToken cancellationToken);
}
