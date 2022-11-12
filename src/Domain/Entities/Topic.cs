using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using GroupProject.Domain.Interfaces;
using GroupProject.Domain.ValueObjects;

namespace GroupProject.Domain.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class Topic : IHasId<Guid>, IHasComplaintCount, IVerifiable
{
    private readonly List<Commentary> _commentaries = new();
    private readonly List<Complaint> _complaints = new();

    /// <summary>
    ///     Parameterless constructor, intended only for orm usage.
    /// </summary>
    private Topic()
    {
    }

    public Topic(
        string header,
        string description,
        CompileOptions? compileOptions,
        Guid userId,
        int sectionId,
        TimeSpan? verificationDuration = null)
    {
        Id = Guid.NewGuid();
        CreationTime = DateTime.UtcNow;
        UserId = userId;
        Header = header;
        Description = description;
        SectionId = sectionId;
        CompileOptions = compileOptions;
        VerifyBefore = DateTime.UtcNow + verificationDuration;
    }

    public static Expression<Func<Topic, bool>> IsOpen => topic => !topic.IsClosed;

    public static Expression<Func<Topic, bool>> IsVerified =>
        topic => topic.VerifyBefore != null && topic.VerifyBefore > DateTime.UtcNow;

    public DateTime CreationTime { get; private set; }
    public string Header { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public bool IsClosed { get; private set; }
    public CompileOptions? CompileOptions { get; private set; }
    public int ViewCount { get; private set; }

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public int SectionId { get; private set; }
    public Section Section { get; private set; } = null!;

    public IEnumerable<Complaint> Complaints => _complaints.ToList();
    public IEnumerable<Commentary> Commentaries => _commentaries.ToList();

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

    public DateTime? VerifyBefore { get; private set; }

    public void Verify()
    {
        VerifyBefore = null;
    }

    public void Close()
    {
        IsClosed = true;
    }

    public void View()
    {
        ViewCount++;
    }
}
