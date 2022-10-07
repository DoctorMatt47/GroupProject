using GroupProject.Application.Common.Interfaces;
using GroupProject.Domain.Entities;
using GroupProject.Domain.Enums;
using GroupProject.Domain.Interfaces;

namespace GroupProject.Infrastructure.Persistence.Initializers;

public class UserInitializer : IEntityInitializer
{
    private readonly IAppDbContext _context;
    private readonly IPasswordHashService _passwordHash;

    public UserInitializer(IAppDbContext context, IPasswordHashService passwordHash)
    {
        _context = context;
        _passwordHash = passwordHash;
    }

    public void Initialize()
    {
        var user = new User("user", "password", _passwordHash, UserRole.User);
        var moderator = new User("moderator", "password", _passwordHash, UserRole.Moderator);
        var admin = new User("admin", "password", _passwordHash, UserRole.Admin);

        _context.Set<User>().Add(user);
        _context.Set<User>().Add(moderator);
        _context.Set<User>().Add(admin);

        _context.SaveChangesAsync().GetAwaiter().GetResult();
    }
}
