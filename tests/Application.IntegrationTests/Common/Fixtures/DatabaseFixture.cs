using AutoFixture;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Domain.Entities;
using GroupProject.Domain.Enums;
using GroupProject.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GroupProject.Application.IntegrationTests.Common.Fixtures;

public class DatabaseFixture : IDisposable
{
    private readonly IPasswordHashService _passwordHash;
    private readonly IServiceScopeFactory _scopeFactory;

    public DatabaseFixture(
        IServiceScopeFactory scopeFactory,
        IPasswordHashService passwordHash)
    {
        _scopeFactory = scopeFactory;
        _passwordHash = passwordHash;

        Initialize().GetAwaiter().GetResult();
    }

    public User DefaultUser { get; private set; } = null!;
    public Section DefaultSection { get; private set; } = null!;

    public void Dispose()
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<IAppDbContext>()!;

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }

    private async Task Initialize()
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<IAppDbContext>()!;

        var fixture = new Fixture();

        DefaultSection = new Section(fixture.Create<string>(), fixture.Create<string>());
        await dbContext.Set<Section>().AddAsync(DefaultSection);

        DefaultUser = new User(fixture.Create<string>(), fixture.Create<string>(), _passwordHash, UserRole.User);
        dbContext.Set<User>().Add(DefaultUser);

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }
}
