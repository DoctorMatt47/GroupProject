using GroupProject.Domain.Enums;

namespace GroupProject.Domain.Entities;

public class Complaint
{
    /// <summary>
    ///     Parameterless constructor, intended only for orm usage.
    /// </summary>
    protected Complaint()
    {
    }

    public Complaint(string description, ComplaintTarget target, Guid elementId)
    {
        Id = Guid.NewGuid();
        CreationTime = DateTime.UtcNow;
        Description = description;
        Target = target;

        _ = target switch
        {
            ComplaintTarget.Topic => TopicId = elementId,
            ComplaintTarget.Commentary => CommentaryId = elementId,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null),
        };
    }

    public Guid Id { get; protected set; }
    public DateTime CreationTime { get; protected set; }
    public string Description { get; protected set; } = null!;
    public ComplaintTarget Target { get; protected set; }

    public Guid? TopicId { get; protected set; }
    public Topic? Topic { get; protected set; }

    public Guid? CommentaryId { get; protected set; }
    public Commentary? Commentary { get; protected set; }
}
