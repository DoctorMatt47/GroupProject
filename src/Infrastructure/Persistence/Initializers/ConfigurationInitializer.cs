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

        var configuration = new Configuration
        {
            Rules = string.Empty,
            BanDuration = TimeSpan.FromMinutes(2),
            WarningCountForBan = 2,
        };

        _dbContext.Set<Configuration>().Add(configuration);
        _dbContext.SaveChangesAsync().GetAwaiter().GetResult();
    }
}
