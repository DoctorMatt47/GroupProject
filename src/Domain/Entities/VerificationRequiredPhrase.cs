using System.Diagnostics.CodeAnalysis;
using GroupProject.Domain.Interfaces;

namespace GroupProject.Domain.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class VerificationRequiredPhrase : IHasId<int>
{
    protected VerificationRequiredPhrase()
    {
    }

    public VerificationRequiredPhrase(string phrase) => Phrase = phrase;
    public string Phrase { get; private set; } = null!;

    public int Id { get; private set; } = 0;
}
