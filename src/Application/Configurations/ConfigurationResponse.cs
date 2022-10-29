namespace GroupProject.Application.Configurations;

public record ConfigurationResponse(string Rules, int WarningCountForBan, TimeSpan BanDuration);
