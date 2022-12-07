namespace GroupProject.Infrastructure.Persistence.Initializers;

public class ConfigurationOptions
{
    public string Rules { get; set; } = null!;
    public int WarningCountForBan { get; set; }
    public TimeSpan BanDuration { get; set; }
    public TimeSpan VerificationDuration { get; set; }
    public IEnumerable<string> ForbiddenPhrases { get; set; } = null!;
    public IEnumerable<string> VerificationRequiredPhrases { get; set; } = null!;
    public IEnumerable<SectionOptions> Sections { get; set; } = null!;
}

public class SectionOptions
{
    public string Header { get; set; } = null!;
    public string Description { get; set; } = null!;
}

