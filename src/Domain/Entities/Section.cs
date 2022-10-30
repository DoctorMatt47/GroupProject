using System.Diagnostics.CodeAnalysis;

namespace GroupProject.Domain.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class Section
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

    public int Id { get; private set; } = 0;
    public string Header { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    public IEnumerable<Topic> Topics => _topics.ToList();
}
