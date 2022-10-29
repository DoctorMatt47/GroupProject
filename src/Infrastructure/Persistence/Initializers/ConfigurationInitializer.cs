using GroupProject.Application.Common.Interfaces;
using GroupProject.Domain.Entities;

namespace GroupProject.Infrastructure.Persistence.Initializers;

public class ConfigurationInitializer : IEntityInitializer
{
    private readonly IAppDbContext _dbContext;

    public ConfigurationInitializer(IAppDbContext dbContext) => _dbContext = dbContext;

    public void Initialize()
    {
        if (_dbContext.Set<Configuration>().Any()) return;

        var configuration = (Configuration) Activator.CreateInstance(typeof(Configuration))!;

        configuration.Rules = string.Empty;
        configuration.BanDuration = TimeSpan.FromMinutes(2);
        configuration.WarningCountForBan = 2;

        _dbContext.Set<Configuration>().Add(configuration);
        _dbContext.SaveChangesAsync().GetAwaiter().GetResult();
    }
}
