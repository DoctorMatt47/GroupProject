using System.Diagnostics.CodeAnalysis;

namespace GroupProject.Domain.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class Configuration
{
    public int Id { get; private set; } = 0;
    public string Rules { get; set; } = null!;
    public int WarningCountForBan { get; set; }
    public TimeSpan BanDuration { get; set; }
    public TimeSpan VerificationDuration { get; set; }
}
