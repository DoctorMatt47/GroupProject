using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GroupProject.Infrastructure.HostedServices;

//TODO Implement
public class DeleteExpiredDataHostedService : IHostedService
{
    private readonly ILogger<DeleteExpiredDataHostedService> _logger;
    private readonly IServiceProvider _provider;

    public DeleteExpiredDataHostedService(IServiceProvider provider, ILogger<DeleteExpiredDataHostedService> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }
}
