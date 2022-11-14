using AutoFixture;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Domain.Entities;
using GroupProject.Domain.Enums;
using GroupProject.Domain.Interfaces;
using GroupProject.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace GroupProject.Application.IntegrationTests.Common.Fixtures;

public class DatabaseFixture
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

    private async Task Initialize()
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<IAppDbContext>()!;

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var fixture = new Fixture();

        DefaultSection = new Section(fixture.Create<string>(), fixture.Create<string>());
        await dbContext.Set<Section>().AddAsync(DefaultSection);

        DefaultUser = new User(fixture.Create<string>(), fixture.Create<string>(), _passwordHash, UserRole.User);
        dbContext.Set<User>().Add(DefaultUser);

        var configuration = new Configuration
        {
            Rules = "",
            BanDuration = TimeSpan.Zero,
            WarningCountForBan = int.MaxValue,
        };

        dbContext.Set<Configuration>().Add(configuration);
        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task<User> CreateUserAsync(UserRole role = UserRole.User)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<IAppDbContext>()!;

        var fixture = new Fixture();
        var user = new User(fixture.Create<string>(), fixture.Create<string>(), _passwordHash, role);

        dbContext.Set<User>().Add(user);
        await dbContext.SaveChangesAsync();

        return user;
    }

    public Task<Topic> CreateTopicAsync() => CreateTopicAsync(DefaultUser.Id, DefaultSection.Id);

    public async Task<Topic> CreateTopicAsync(Guid userId, int sectionId)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<IAppDbContext>()!;

        var fixture = new Fixture();
        var topic = new Topic(
            fixture.Create<string>(),
            fixture.Create<string>(),
            fixture.Create<CompileOptions>(),
            userId,
            sectionId,
            fixture.Create<TimeSpan>());

        dbContext.Set<Topic>().Add(topic);
        await dbContext.SaveChangesAsync();

        return topic;
    }

    public async Task<Section> CreateSectionAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<IAppDbContext>()!;

        var fixture = new Fixture();
        var section = new Section(fixture.Create<string>(), fixture.Create<string>());

        await dbContext.Set<Section>().AddAsync(section);
        await dbContext.SaveChangesAsync();

        return section;
    }
}
