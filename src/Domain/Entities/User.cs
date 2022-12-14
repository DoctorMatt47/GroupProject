using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using GroupProject.Domain.Enums;
using GroupProject.Domain.Interfaces;

namespace GroupProject.Domain.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class User : IHasId<Guid>
{
    private readonly List<Commentary> _commentaries = new();
    private readonly List<Topic> _questions = new();

    /// <summary>
    ///     Parameterless constructor, intended only for orm usage.
    /// </summary>
    private User()
    {
    }

    public User(string login, string password, IPasswordHashService passwordHash, UserRole role)
    {
        Id = Guid.NewGuid();
        WarningCount = 0;
        CreationTime = DateTime.UtcNow;
        Login = login;
        PasswordSalt = passwordHash.GenerateSalt();
        PasswordHash = passwordHash.Encode(password, PasswordSalt);
        Role = role;
    }

    public static Expression<Func<User, bool>> IsBanned => user => user.BanEndTime > DateTime.UtcNow;

    public string Login { get; private set; } = null!;
    public byte[] PasswordHash { get; private set; } = null!;
    public byte[] PasswordSalt { get; private set; } = null!;
    public UserRole Role { get; set; } = UserRole.User;
    public DateTime CreationTime { get; set; }
    public DateTime? BanEndTime { get; private set; }
    public int WarningCount { get; private set; }

    public IEnumerable<Topic> Topics => _questions.ToList();
    public IEnumerable<Commentary> Commentaries => _commentaries.ToList();

    public Guid Id { get; private set; }

    public void AddWarning(int minWarningCountForBan, TimeSpan banDuration)
    {
        WarningCount++;
        if (WarningCount < minWarningCountForBan) return;
        SetBanned(banDuration);
    }

    public void SetBanned(TimeSpan banDuration)
    {
        BanEndTime = (BanEndTime > DateTime.UtcNow ? BanEndTime : DateTime.UtcNow) + banDuration;
    }

    public void SetUnbanned()
    {
        BanEndTime = null;
    }
}
