using System.Diagnostics.CodeAnalysis;
using GroupProject.Domain.Interfaces;
using GroupProject.Domain.ValueObjects;

namespace GroupProject.Domain.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class Commentary : IHasComplaintCount, IHasId<Guid>
{
    private readonly List<Complaint> _complaints = new();

    /// <summary>
    ///     Parameterless constructor, intended only for orm usage.
    /// </summary>
    private Commentary()
    {
    }

    public Commentary(string description, CompileOptions? compileOptions, Guid topicId, Guid userId)
    {
        Id = Guid.NewGuid();
        CreationTime = DateTime.UtcNow;
        Description = description;
        CompileOptions = compileOptions;
        TopicId = topicId;
        UserId = userId;
    }

    public DateTime CreationTime { get; private set; }
    public string Description { get; private set; } = null!;
    public CompileOptions? CompileOptions { get; private set; }

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

    public Guid Id { get; private set; }
}
