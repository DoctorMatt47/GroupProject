using GroupProject.Domain.Enums;
using GroupProject.Domain.Interfaces;

namespace GroupProject.Domain.Entities;

public class User
{
    /// <summary>
    ///     Parameterless constructor, intended only for orm usage.
    /// </summary>
    protected User()
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

    public Guid Id { get; protected set; }
    public string Login { get; protected set; } = null!;
    public byte[] PasswordHash { get; protected set; } = null!;
    public byte[] PasswordSalt { get; protected set; } = null!;
    public UserRole Role { get; set; } = UserRole.User;
    public UserStatus Status { get; set; } = UserStatus.Active;

    public IEnumerable<Topic> Topics => _questions.ToList();
    public IEnumerable<Commentary> Commentaries => _commentaries.ToList();

    // ReSharper disable CollectionNeverUpdated.Local
    private readonly List<Commentary> _commentaries = new();

    private readonly List<Topic> _questions = new();
    // ReSharper restore CollectionNeverUpdated.Local
}
