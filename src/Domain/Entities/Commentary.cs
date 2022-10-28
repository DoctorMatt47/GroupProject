using System.Diagnostics.CodeAnalysis;
using GroupProject.Domain.Interfaces;

namespace GroupProject.Domain.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class Commentary : IHasComplaintCount
{
    private readonly List<Complaint> _complaints = new();

    /// <summary>
    ///     Parameterless constructor, intended only for orm usage.
    /// </summary>
    private Commentary()
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

    public Guid Id { get; private set; }
    public DateTime CreationTime { get; private set; }
    public string Description { get; private set; } = null!;
    public string? Code { get; private set; }

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public Guid TopicId { get; private set; }
    public Topic Topic { get; private set; } = null!;

    public IEnumerable<Complaint> Complaints => _complaints.ToList();
    public int ComplaintCount { get; private set; }

    public void IncrementComplaintCount()
    {
        ComplaintCount++;
    }

    public void DecrementComplaintCount()
    {
        if (ComplaintCount != 0) ComplaintCount--;
    }
}
