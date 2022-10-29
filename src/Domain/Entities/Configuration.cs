using System.Diagnostics.CodeAnalysis;

namespace GroupProject.Domain.Entities;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class Configuration
{
    private Configuration()
    {
    }

    public string Rules { get; set; } = null!;
    public int WarningCountForBan { get; set; }
    public TimeSpan BanDuration { get; set; }
}
