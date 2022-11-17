using AutoMapper;
using AutoMapper.QueryableExtensions;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Extensions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Common.Requests;
using GroupProject.Application.Common.Responses;
using GroupProject.Application.Configurations;
using GroupProject.Application.Phrases;
using GroupProject.Domain.Entities;
using GroupProject.Domain.Enums;
using GroupProject.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupProject.Application.Users;

public class UserService : IUserService
{
    private readonly IConfigurationService _configuration;
    private readonly IAppDbContext _dbContext;
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IPasswordHashService _passwordHash;
    private readonly IPhraseService _phrases;

    public UserService(
        IAppDbContext dbContext,
        IConfigurationService configuration,
        IPasswordHashService passwordHash,
        ILogger<UserService> logger,
        IMapper mapper,
        IPhraseService phrases)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _passwordHash = passwordHash;
        _logger = logger;
        _mapper = mapper;
        _phrases = phrases;
    }

    public async Task<Page<UserResponse>> GetModerators(PageRequest request, CancellationToken cancellationToken) =>
        await GetUsersByRoleAsync(UserRole.Moderator, request, cancellationToken);

    public async Task<Page<UserResponse>> GetUsers(PageRequest request, CancellationToken cancellationToken) =>
        await GetUsersByRoleAsync(UserRole.User, request, cancellationToken);

    public async Task<IEnumerable<UserResponse>> GetBannedUsers(CancellationToken cancellationToken) =>
        await _dbContext.Set<User>()
            .Where(User.IsBanned)
            .ProjectTo<UserResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

    public async Task<UserResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Set<User>().FindOrThrowAsync(id, cancellationToken);
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
        var user = await _dbContext.Set<User>().FindOrThrowAsync(id, cancellationToken);
        var configuration = await _dbContext.Set<Configuration>().FirstAsync(cancellationToken);

        user.AddWarning(configuration.WarningCountForBan, configuration.BanDuration);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task BanUser(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Set<User>().FindOrThrowAsync(id, cancellationToken);
        if (user.Role is not UserRole.User) throw new BadRequestException("You can't ban moderator or admin");

        var configuration = await _configuration.Get(cancellationToken);
        user.SetBanned(configuration.BanDuration);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private Task<Page<UserResponse>> GetUsersByRoleAsync(
        UserRole role,
        PageRequest request,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<User>()
            .Where(u => u.Role == role)
            .OrderByDescending(u => u.CreationTime)
            .ProjectTo<UserResponse>(_mapper.ConfigurationProvider)
            .ToPageAsync(request, cancellationToken);
    }

    private async Task<IdResponse<Guid>> CreateUserImplAsync(
        CreateUserRequest request,
        UserRole role,
        CancellationToken cancellationToken)
    {
        await _dbContext.Set<User>().NoOneOrThrowAsync(
            u => u.Login == request.Login,
            $"login: {request.Login}",
            cancellationToken);

        var containsForbidden = (await _phrases
                .GetForbidden(cancellationToken))
            .Any(p => _phrases.ContainsPhrase(request.Login, p.Phrase));

        if (containsForbidden)
            throw new BadRequestException($"Login contains forbidden words: {string.Join(',', containsForbidden)}");

        var user = new User(request.Login, request.Password, _passwordHash, role);

        _dbContext.Set<User>().Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created {Role} with id: {Id}", role, user.Id);

        return new IdResponse<Guid>(user.Id);
    }
}
