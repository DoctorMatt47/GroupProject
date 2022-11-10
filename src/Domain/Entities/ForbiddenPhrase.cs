using System.Diagnostics.CodeAnalysis;
using GroupProject.Domain.Interfaces;

namespace GroupProject.Domain.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class ForbiddenPhrase : IHasId<int>
{
    protected ForbiddenPhrase()
    {
    }

    public ForbiddenPhrase(string phrase) => Phrase = phrase;
    public string Phrase { get; private set; } = null!;

    public int Id { get; private set; } = 0;
}
