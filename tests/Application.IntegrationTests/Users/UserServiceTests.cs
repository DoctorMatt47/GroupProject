using AutoFixture;
using FluentAssertions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.IntegrationTests.Common.Fixtures;
using GroupProject.Application.Users;
using GroupProject.Domain.Entities;
using GroupProject.Domain.Enums;

namespace GroupProject.Application.IntegrationTests.Users;

public class UserServiceTests
{
    private readonly DatabaseFixture _db;
    private readonly IAppDbContext _dbContext;
    private readonly IUserService _users;

    public UserServiceTests(
        DatabaseFixture db,
        IAppDbContext dbContext,
        IUserService users)
    {
        _db = db;
        _dbContext = dbContext;
        _users = users;
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
}
