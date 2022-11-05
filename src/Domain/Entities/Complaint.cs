using System.Diagnostics.CodeAnalysis;
using GroupProject.Domain.Enums;
using GroupProject.Domain.Interfaces;

namespace GroupProject.Domain.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class Complaint : IHasId<Guid>
{
    /// <summary>
    ///     Parameterless constructor, intended only for orm usage.
    /// </summary>
    private Complaint()
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

    public DateTime CreationTime { get; private set; }
    public string Description { get; private set; } = null!;
    public ComplaintTarget Target { get; private set; }

    public Guid? TopicId { get; private set; }
    public Topic? Topic { get; private set; } = null!;

    public Guid? CommentaryId { get; private set; }
    public Commentary? Commentary { get; private set; } = null!;

    public Guid Id { get; private set; }
}
