using GroupProject.Infrastructure.Persistence.Initializers;

namespace GroupProject.WebApi.Extensions;

public static class ServiceProviderExtensions
{
    internal static void CallEntityInitializers(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var initializers = scope.ServiceProvider.GetServices<IEntityInitializer>();
        foreach (var initializer in initializers) initializer.Initialize();
    }
}
