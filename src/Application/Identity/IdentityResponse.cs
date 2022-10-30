namespace GroupProject.Application.Identity;

public record IdentityResponse(
    string? Token,
    Guid Id,
    string Role,
    DateTime? BanEndTime);
