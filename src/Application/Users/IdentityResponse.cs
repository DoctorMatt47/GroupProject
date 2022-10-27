namespace GroupProject.Application.Users;

public record IdentityResponse(
    string Token,
    Guid Id,
    string Role,
    string Status);
