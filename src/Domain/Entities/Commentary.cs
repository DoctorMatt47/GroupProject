using System.Diagnostics.CodeAnalysis;
using GroupProject.Domain.Interfaces;

namespace GroupProject.Domain.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
public class Commentary : IHasComplaintCount
{
    private readonly List<Complaint> _complaints = new();

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
    public Topic Topic { get; protected set; } = null!;

    public IEnumerable<Complaint> Complaints => _complaints.ToList();
    public int ComplaintCount { get; protected set; }

    public void IncrementComplaintCount()
    {
        ComplaintCount++;
    }

    public void DecrementComplaintCount()
    {
        if (ComplaintCount != 0) ComplaintCount--;
    }
}
