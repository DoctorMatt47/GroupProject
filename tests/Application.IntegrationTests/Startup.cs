using GroupProject.Application.Common.Extensions;
using GroupProject.Application.IntegrationTests.Common.Fixtures;
using GroupProject.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupProject.Application.IntegrationTests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Testing.json", true, true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services
            .AddApplication()
            .AddInfrastructure(connectionString)
            .AddSingleton<DatabaseFixture>();
    }
}
