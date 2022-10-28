﻿using System.Diagnostics.CodeAnalysis;
using GroupProject.Domain.Enums;
using GroupProject.Domain.Interfaces;

namespace GroupProject.Domain.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class Topic : IHasComplaintCount
{
    private readonly List<Commentary> _commentaries = new();
    private readonly List<Complaint> _complaints = new();

    /// <summary>
    ///     Parameterless constructor, intended only for orm usage.
    /// </summary>
    private Topic()
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

    public Guid Id { get; private set; }
    public DateTime CreationTime { get; private set; }
    public string Header { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string? Code { get; private set; }
    public TopicStatus Status { get; private set; } = TopicStatus.Active;

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
}
