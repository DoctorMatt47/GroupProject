using AutoMapper;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupProject.Application.Configurations;

public class ConfigurationService : IConfigurationService
{
    private readonly IAppDbContext _dbContext;
    private readonly ILogger<ConfigurationService> _logger;
    private readonly IMapper _mapper;

    public ConfigurationService(
        IAppDbContext dbContext,
        ILogger<ConfigurationService> logger,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<ConfigurationResponse> Get(CancellationToken cancellationToken)
    {
        var configuration = await _dbContext.Set<Configuration>().FirstAsync(cancellationToken);
        return _mapper.Map<ConfigurationResponse>(configuration);
    }

    public async Task Patch(PatchConfigurationRequest request, CancellationToken cancellationToken)
    {
        var configuration = await _dbContext.Set<Configuration>().FirstAsync(cancellationToken);

        if (request.Rules is not null) configuration.Rules = request.Rules;
        if (request.BanDuration is not null) configuration.BanDuration = request.BanDuration.Value;
        if (request.WarningCountForBan is not null) configuration.WarningCountForBan = request.WarningCountForBan.Value;
        if (request.ComplaintDuration is not null) configuration.VerificationDuration = request.ComplaintDuration.Value;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Patched configuration settings");
    }
}
