namespace GroupProject.Application.Configurations;

public record PatchConfigurationRequest(
    string? Rules,
    int? WarningCountForBan,
    TimeSpan? BanDuration,
    TimeSpan? ComplaintDuration);
