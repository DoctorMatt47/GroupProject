using AutoMapper;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Extensions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Common.Responses;
using GroupProject.Domain.Entities;
using GroupProject.Domain.Enums;
using GroupProject.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupProject.Application.Users;

public class UserService : IUserService
{
    private readonly IAppDbContext _dbContext;
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IPasswordHashService _passwordHash;

    public UserService(
        IAppDbContext dbContext,
        IPasswordHashService passwordHash,
        ILogger<UserService> logger,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _passwordHash = passwordHash;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<UserResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Set<User>().AssertFoundAsync(id, cancellationToken);
        return _mapper.Map<UserResponse>(user);
    }

    public async Task<IdResponse<Guid>> CreateUser(
        CreateUserRequest request,
        CancellationToken cancellationToken) =>
        await CreateUserImplAsync(request, UserRole.User, cancellationToken);

    public async Task<IdResponse<Guid>> CreateModerator(
        CreateUserRequest request,
        CancellationToken cancellationToken) =>
        await CreateUserImplAsync(request, UserRole.Moderator, cancellationToken);

    public async Task AddWarningToUser(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Set<User>().AssertFoundAsync(id, cancellationToken);
        var configuration = await _dbContext.Set<Configuration>().FirstAsync(cancellationToken);

        user.AddWarning(configuration.WarningCountForBan, configuration.BanDuration);
    }

    private async Task<IdResponse<Guid>> CreateUserImplAsync(
        CreateUserRequest request,
        UserRole role,
        CancellationToken cancellationToken)
    {
        var isUserExist = await _dbContext.Set<User>().AnyAsync(u => u.Login == request.Login, cancellationToken);
        if (isUserExist) throw new ConflictException($"There is already user with login: {request.Login}");

        var user = new User(request.Login, request.Password, _passwordHash, role);

        _dbContext.Set<User>().Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created {Role} with id: {Id}", role, user.Id);

        return new IdResponse<Guid>(user.Id);
    }
}
