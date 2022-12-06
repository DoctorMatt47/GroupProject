namespace GroupProject.Infrastructure.Persistence.Initializers;

public class ConfigurationOptions
{
    public string Rules { get; set; } = null!;
    public int WarningCountForBan { get; set; }
    public TimeSpan BanDuration { get; set; }
    public TimeSpan VerificationDuration { get; set; }
    public IEnumerable<string> ForbiddenPhrases { get; set; } = null!;
    public IEnumerable<string> VerificationRequiredPhrases { get; set; } = null!;
}
