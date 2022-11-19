using AutoMapper;
using GroupProject.Application.Common.Exceptions;
using GroupProject.Application.Common.Interfaces;
using GroupProject.Domain.Entities;
using GroupProject.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupProject.Application.Identity;

public class IdentityService : IIdentityService
{
    private readonly IAppDbContext _dbContext;
    private readonly IIdentityRepository _identities;
    private readonly ILogger<IdentityService> _logger;
    private readonly IMapper _mapper;
    private readonly IPasswordHashService _passwordHash;
    private readonly ITokenRepository _tokens;

    public IdentityService(
        IAppDbContext dbContext,
        IIdentityRepository identities,
        ILogger<IdentityService> logger,
        IPasswordHashService passwordHash,
        IMapper mapper,
        ITokenRepository tokens)
    {
        _dbContext = dbContext;
        _identities = identities;
        _logger = logger;
        _passwordHash = passwordHash;
        _mapper = mapper;
        _tokens = tokens;
    }

    public async Task<IdentityResponse> Create(
        CreateIdentityRequest request,
        CancellationToken cancellationToken)
    {
        const string exceptionMessage = "Incorrect password or login";

        var user = await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.Login == request.Login, cancellationToken);
        if (user is null) throw new BadRequestException(exceptionMessage);

        var passwordHash = _passwordHash.Encode(request.Password, user.PasswordSalt);
        if (!user.PasswordHash.SequenceEqual(passwordHash)) throw new BadRequestException(exceptionMessage);

        _logger.LogInformation("Authenticated {Role} with id: {Id}", user.Role, user.Id);

        var identity = _identities.Create(_mapper.Map<Identity>(user));
        var token = _tokens.Get(identity);

        return new IdentityResponse(token, user.Id, Enum.GetName(user.Role)!, user.BanEndTime);
    }
}
