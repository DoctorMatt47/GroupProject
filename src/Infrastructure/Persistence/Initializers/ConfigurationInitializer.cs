using GroupProject.Application.Common.Interfaces;
using GroupProject.Domain.Entities;
using Microsoft.Extensions.Options;

namespace GroupProject.Infrastructure.Persistence.Initializers;

public class ConfigurationInitializer : IEntityInitializer
{
    private readonly IAppDbContext _dbContext;
    private readonly ConfigurationOptions _options;

    public ConfigurationInitializer(IAppDbContext dbContext, IOptions<ConfigurationOptions> options)
    {
        _dbContext = dbContext;
        _options = options.Value;
    }

    public void Initialize()
    {
        if (_dbContext.Set<Configuration>().Any()) return;

        var configuration = new Configuration
        {
            Rules = _options.Rules,
            BanDuration = _options.BanDuration,
            WarningCountForBan = _options.WarningCountForBan,
            VerificationDuration = _options.VerificationDuration,
        };

        var requiredPhrases = _options.VerificationRequiredPhrases.Select(p => new VerificationRequiredPhrase(p));
        _dbContext.Set<VerificationRequiredPhrase>().AddRange(requiredPhrases);

        var forbiddenPhrases = _options.ForbiddenPhrases.Select(p => new ForbiddenPhrase(p));
        _dbContext.Set<ForbiddenPhrase>().AddRange(forbiddenPhrases);

        _dbContext.Set<Configuration>().Add(configuration);
        _dbContext.SaveChangesAsync().GetAwaiter().GetResult();
    }
}
