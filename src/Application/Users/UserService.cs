using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Application.Identity;
using GroupProject.Domain.Entities;
using GroupProject.Domain.Enums;
using GroupProject.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupProject.Application.Users;

public class UserService : IUserService
{
    private readonly IAppContext _context;
    private readonly IJwtTokenService _jwtToken;
    private readonly ILogger _logger;
    private readonly IPasswordHashService _passwordHash;

    public UserService(
        IAppContext context,
        IJwtTokenService jwtToken,
        IPasswordHashService passwordHash,
        ILogger logger)
    {
        _context = context;
        _jwtToken = jwtToken;
        _passwordHash = passwordHash;
        _logger = logger;
    }

    public async Task<AuthenticateUserResponse> CreateUser(
        CreateUserRequest request,
        CancellationToken cancellationToken) =>
        await CreateUserImplAsync(request, UserRole.User, cancellationToken);

    public async Task<AuthenticateUserResponse> CreateModerator(
        CreateUserRequest request,
        CancellationToken cancellationToken) =>
        await CreateUserImplAsync(request, UserRole.Moderator, cancellationToken);

    public async Task<AuthenticateUserResponse> Authenticate(
        AuthenticateUserRequest request,
        CancellationToken cancellationToken)
    {
        var badRequestException = new BadRequestException("Incorrect password or login");

        var user = await _context.Set<User>().FirstOrDefaultAsync(u => u.Login == request.Login, cancellationToken);
        if (user is null) throw badRequestException;

        var passwordHash = _passwordHash.Encode(request.Password, user.PasswordSalt);
        if (!user.PasswordHash.SequenceEqual(passwordHash)) throw badRequestException;

        _logger.LogInformation("Authenticated {Role} with id: {Id}", user.Role, user.Id);

        var token = _jwtToken.Get(user.Id, user.Role);
        return new AuthenticateUserResponse(token, user.Id, Enum.GetName(user.Role)!, Enum.GetName(user.Status)!);
    }

    private async Task<AuthenticateUserResponse> CreateUserImplAsync(
        CreateUserRequest request,
        UserRole role,
        CancellationToken cancellationToken)
    {
        var isUserExist = await _context.Set<User>().AnyAsync(u => u.Login == request.Login, cancellationToken);
        if (isUserExist) throw new ConflictException($"There is already user with login: {request.Login}");

        var user = new User(request.Login, request.Password, _passwordHash, role);
        _context.Set<User>().Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created {Role} with id: {Id}", role, user.Id);

        var token = _jwtToken.Get(user.Id, user.Role);
        return new AuthenticateUserResponse(token, user.Id, Enum.GetName(user.Role)!, Enum.GetName(user.Status)!);
    }
}
