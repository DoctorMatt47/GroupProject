using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using GroupProject.Domain.Interfaces;
using GroupProject.Domain.ValueObjects;

namespace GroupProject.Domain.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class Commentary : IHasId<Guid>, IVerifiable
{
    private readonly List<Complaint> _complaints = new();

    /// <summary>
    ///     Parameterless constructor, intended only for orm usage.
    /// </summary>
    private Commentary()
    {
    }

    public Commentary(
        string description,
        CompileOptions? compileOptions,
        Guid topicId,
        Guid userId,
        TimeSpan? verificationDuration)
    {
        Id = Guid.NewGuid();
        CreationTime = DateTime.UtcNow;
        Description = description;
        CompileOptions = compileOptions;
        TopicId = topicId;
        UserId = userId;
        VerifyBefore = DateTime.UtcNow + verificationDuration;
    }

    public static Expression<Func<Commentary, bool>> VerificationRequired =>
        topic => topic.VerifyBefore != null && topic.VerifyBefore > DateTime.UtcNow;

    public DateTime CreationTime { get; private set; }
    public string Description { get; private set; } = null!;
    public CompileOptions? CompileOptions { get; private set; }
    public int ComplaintCount { get; private set; } = 0;

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public Guid TopicId { get; private set; }
    public Topic Topic { get; private set; } = null!;

    public IEnumerable<Complaint> Complaints => _complaints.ToList();

    public Guid Id { get; private set; }

    public DateTime? VerifyBefore { get; private set; }

    public void SetVerified()
    {
        VerifyBefore = null;
    }
}
