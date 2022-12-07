using AutoFixture;
using FluentAssertions;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.IntegrationTests.Common.Fixtures;
using GroupProject.Application.Users;
using GroupProject.Domain.Entities;
using GroupProject.Domain.Enums;
using GroupProject.Domain.Interfaces;

namespace GroupProject.Application.IntegrationTests.Users;

public class UserServiceTests
{
    private readonly DatabaseFixture _db;
    private readonly IAppDbContext _dbContext;
    private readonly IPasswordHashService _passwordHash;
    private readonly IUserService _users;

    public UserServiceTests(
        DatabaseFixture db,
        IAppDbContext dbContext,
        IUserService users,
        IPasswordHashService passwordHash)
    {
        _db = db;
        _dbContext = dbContext;
        _users = users;
        _passwordHash = passwordHash;
    }

    [Fact]
    public async Task GetUser()
    {
        var user = User();
        _dbContext.Set<User>().Add(user);
        await _dbContext.SaveChangesAsync();

        var response = await _users.Get(user.Id, CancellationToken.None);
        response.Login.Should().Be(user.Login);
        response.Role.Should().Be(Enum.GetName(user.Role));
        response.CreationTime.Should().Be(user.CreationTime);
    }

    [Fact]
    public async Task CreateUser()
    {
        var fixture = new Fixture();
        var request = new CreateUserRequest(fixture.Create<string>(), fixture.Create<string>());

        var response = await _users.CreateUser(request, CancellationToken.None);
        var user = await _dbContext.Set<User>().FindAsync(response.Id);

        user.Should().NotBeNull();
        user!.Login.Should().Be(request.Login);
        user.Role.Should().Be(UserRole.User);
    }

    [Fact]
    public async Task CreateModerator()
    {
        var fixture = new Fixture();
        var request = new CreateUserRequest(fixture.Create<string>(), fixture.Create<string>());

        var response = await _users.CreateModerator(request, CancellationToken.None);
        var user = await _dbContext.Set<User>().FindAsync(response.Id);

        user.Should().NotBeNull();
        user!.Login.Should().Be(request.Login);
        user.Role.Should().Be(UserRole.Moderator);
    }

    [Fact]
    public async Task AddWarningToUser()
    {
        var user = User();
        _dbContext.Set<User>().Add(user);
        await _dbContext.SaveChangesAsync();

        await _users.AddWarningToUser(user.Id, CancellationToken.None);
        user.WarningCount.Should().Be(1);

        await _users.AddWarningToUser(user.Id, CancellationToken.None);
        user.WarningCount.Should().Be(2);
    }

    [Fact]
    public async Task AddWarningToUser_ThrowExceptionIfNotFound()
    {
        await FluentActions
            .Invoking(() => _users.AddWarningToUser(Guid.NewGuid(), CancellationToken.None))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    private User User()
    {
        var fixture = new Fixture();
        return new User(
            fixture.Create<string>(),
            fixture.Create<string>(),
            _passwordHash,
            fixture.Create<UserRole>());
    }
}
