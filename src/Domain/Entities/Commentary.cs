namespace GroupProject.Domain.Entities;

public class Commentary
{
    /// <summary>
    ///     Parameterless constructor, intended only for orm usage.
    /// </summary>
    protected Commentary()
    {
    }

    public Commentary(string description, string? code, Guid topicId, Guid userId)
    {
        Id = Guid.NewGuid();
        CreationTime = DateTime.UtcNow;
        Description = description;
        Code = code;
        TopicId = topicId;
        UserId = userId;
    }

    public Guid Id { get; protected set; }
    public DateTime CreationTime { get; protected set; }
    public string Description { get; protected set; } = null!;
    public string? Code { get; protected set; }

    public Guid UserId { get; protected set; }
    public User User { get; protected set; } = null!;

    public Guid TopicId { get; protected set; }
    public User Topic { get; protected set; } = null!;
}
