using System.Diagnostics.CodeAnalysis;

namespace GroupProject.Domain.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
public class Section
{
    private readonly List<Topic> _topics = new();

    /// <summary>
    ///     Parameterless constructor, intended only for orm usage.
    /// </summary>
    protected Section()
    {
    }

    public Section(string header, string description)
    {
        Header = header;
        Description = description;
    }

    public int Id { get; protected set; }
    public string Header { get; protected set; } = null!;
    public string Description { get; protected set; } = null!;

    public IEnumerable<Topic> Topics => _topics.ToList();
}
