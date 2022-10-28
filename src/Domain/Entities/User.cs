using System.Diagnostics.CodeAnalysis;
using GroupProject.Domain.Enums;
using GroupProject.Domain.Interfaces;

namespace GroupProject.Domain.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class User
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
        Login = login;
        PasswordSalt = passwordHash.GenerateSalt();
        PasswordHash = passwordHash.Encode(password, PasswordSalt);
        Role = role;
    }

    public Guid Id { get; private set; }
    public string Login { get; private set; } = null!;
    public byte[] PasswordHash { get; private set; } = null!;
    public byte[] PasswordSalt { get; private set; } = null!;
    public UserRole Role { get; set; } = UserRole.User;
    public UserStatus Status { get; set; } = UserStatus.Active;

    public IEnumerable<Topic> Topics => _questions.ToList();
    public IEnumerable<Commentary> Commentaries => _commentaries.ToList();
}
