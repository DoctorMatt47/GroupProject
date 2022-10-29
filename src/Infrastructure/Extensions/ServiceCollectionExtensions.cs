using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Identity;
using GroupProject.Infrastructure.Identity;
using GroupProject.Infrastructure.Persistence.Contexts;
using GroupProject.Infrastructure.Persistence.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GroupProject.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<IAppDbContext, AppDbContext>(opts => opts.UseSqlite(connectionString));

        services.AddTransient<ITokenRepository, TokenRepository>();
        services.AddTransient<IIdentityRepository, IdentityRepository>();
        services.AddScoped<IEntityInitializer, UserInitializer>();
        services.AddScoped<IEntityInitializer, ConfigurationInitializer>();

        return services;
    }
}
