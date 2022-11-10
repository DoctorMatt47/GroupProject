using System.Diagnostics.CodeAnalysis;
using GroupProject.Domain.Interfaces;

namespace GroupProject.Domain.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class Section : IHasId<int>
{
    private readonly List<Topic> _topics = new();

    /// <summary>
    ///     Parameterless constructor, intended only for orm usage.
    /// </summary>
    private Section()
    {
    }

    public Section(string header, string description)
    {
        Header = header;
        Description = description;
    }

    public string Header { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public int TopicCount { get; private set; }

    public IEnumerable<Topic> Topics => _topics.ToList();

    public int Id { get; private set; } = 0;

    public void IncrementTopicCount()
    {
        TopicCount++;
    }

    public void DecrementTopicCount()
    {
        TopicCount--;
    }
}
