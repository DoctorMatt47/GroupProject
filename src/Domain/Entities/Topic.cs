using GroupProject.Domain.Enums;

namespace GroupProject.Domain.Entities;

public class Topic
{
    // ReSharper disable once CollectionNeverUpdated.Local
    private readonly List<Complaint> _complaints = new();

    /// <summary>
    ///     Parameterless constructor, intended only for orm usage.
    /// </summary>
    protected Topic()
    {
    }

    public Topic(string header, string description, string? code, Guid userId)
    {
        Id = Guid.NewGuid();
        CreationTime = DateTime.UtcNow;
        UserId = userId;
        Header = header;
        Description = description;
        Code = code;
    }

    public Guid Id { get; protected set; }
    public DateTime CreationTime { get; protected set; }
    public string Header { get; protected set; } = null!;
    public string Description { get; protected set; } = null!;
    public string? Code { get; protected set; }
    public TopicStatus Status { get; protected set; } = TopicStatus.Active;

    public Guid UserId { get; protected set; }
    public User User { get; protected set; } = null!;

    public IEnumerable<Complaint> Complaints => _complaints.ToList();
}
