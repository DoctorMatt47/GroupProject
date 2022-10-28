using System.Diagnostics.CodeAnalysis;
using GroupProject.Domain.Enums;
using GroupProject.Domain.Interfaces;

namespace GroupProject.Domain.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
public class Topic : IHasComplaintCount
{
    private readonly List<Commentary> _commentaries = new();
    private readonly List<Complaint> _complaints = new();

    /// <summary>
    ///     Parameterless constructor, intended only for orm usage.
    /// </summary>
    protected Topic()
    {
    }

    public Topic(string header, string description, string? code, Guid userId, int sectionId)
    {
        Id = Guid.NewGuid();
        CreationTime = DateTime.UtcNow;
        UserId = userId;
        Header = header;
        Description = description;
        Code = code;
        SectionId = sectionId;
    }

    public Guid Id { get; protected set; }
    public DateTime CreationTime { get; protected set; }
    public string Header { get; protected set; } = null!;
    public string Description { get; protected set; } = null!;
    public string? Code { get; protected set; }
    public TopicStatus Status { get; protected set; } = TopicStatus.Active;

    public Guid UserId { get; protected set; }
    public User User { get; protected set; } = null!;

    public int SectionId { get; protected set; }
    public Section Section { get; protected set; } = null!;

    public IEnumerable<Complaint> Complaints => _complaints.ToList();
    public IEnumerable<Commentary> Commentaries => _commentaries.ToList();
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
